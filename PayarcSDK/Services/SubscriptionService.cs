using System.Text.Json;
using AnyOfTypes;
using Newtonsoft.Json;
using PayarcSDK.Entities;
using PayarcSDK.Entities.Billing.Subscriptions;
using PayarcSDK.Http;
using JsonException = System.Text.Json.JsonException;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace PayarcSDK.Services;

public class SubscriptionService
{
    private readonly ApiClient _apiClient;
    private readonly HttpClient _httpClient;

    public SubscriptionService(AnyOf<ApiClient, HttpClient> apiClient)
    {
        _apiClient = apiClient.IsFirst ? apiClient.First : new ApiClient(apiClient.Second);
        _httpClient = apiClient.IsSecond ? apiClient.Second : new HttpClient();
    }
    
    public async Task<ListBaseResponse?> List(SubscriptionListOptions? options = null)
    {
        var parameters = new Dictionary<string, object?>
            {
                { "limit", options?.Limit ?? 9999 },
                { "plan", options?.PlanId },
                { "search", options?.Search }
            }.Where(kvp => kvp.Value != null)
            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            var query = BuildQueryString(parameters);
            return await GetSubsAsync("subscriptions", query);
    }
    
    private string BuildQueryString(Dictionary<string, object?> parameters)
    {
        var queryString = string.Join("&",
            parameters.Select(p =>
                $"{Uri.EscapeDataString(p.Key)}={Uri.EscapeDataString(p.Value?.ToString() ?? string.Empty)}"));
        return queryString;
    }
    
     private async Task<ListBaseResponse?> GetSubsAsync(string endpoint, string? queryParams, string type = "Subscription")
    {
        try
        {
            var url = $"{endpoint}?{queryParams}";
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var response = await _httpClient.SendAsync(request);
            var responseBody = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Response status code: {response.StatusCode}");
            if (!response.IsSuccessStatusCode)
            {
                var errorData = JsonSerializer.Deserialize<Dictionary<string, object>>(responseBody);
                Console.WriteLine($"Error details: {JsonSerializer.Serialize(errorData)}");
                throw new InvalidOperationException($"HTTP error {response.StatusCode}: {responseBody}");
            }

            if (string.IsNullOrWhiteSpace(responseBody))
            {
                throw new InvalidOperationException("Response body is empty.");
            }
            var responseData = JsonSerializer.Deserialize<Dictionary<string, object>>(responseBody);
            if (responseData == null || !responseData.TryGetValue("data", out var dataValue) ||
                !(dataValue is JsonElement dataElement))
            {
                throw new InvalidOperationException("Response data is invalid or missing.");
            }

            var rawData = dataElement.GetRawText();
            var jsonSubs = dataElement.Deserialize<List<Dictionary<string, object>>>();
            List<BaseResponse?>? subs = new List<BaseResponse?>();
            if (jsonSubs != null)
            {
                for (int i = 0; i < jsonSubs.Count; i++)
                {
                    var ch = TransformJsonRawObject(jsonSubs[i], JsonSerializer.Serialize(jsonSubs[i]), type);
                    subs?.Add(ch);
                }
            }
            var pagination = new Dictionary<string, object>();
            if (responseData.TryGetValue("meta", out var metaValue) && metaValue is JsonElement metaElement)
            {
                var paginationElement = metaElement.GetProperty("pagination");
                pagination["total"] = paginationElement.GetProperty("total").GetInt32();
                pagination["count"] = paginationElement.GetProperty("count").GetInt32();
                pagination["per_page"] = paginationElement.GetProperty("per_page").GetInt32();
                pagination["current_page"] = paginationElement.GetProperty("current_page").GetInt32();
                pagination["total_pages"] = paginationElement.GetProperty("total_pages").GetInt32();
            }

            pagination?.Remove("links");
            return new SubscriptionListResponse
            {
                Data = subs,
                Pagination = pagination,
                RawData = rawData
            };
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"HTTP error processing charge: {ex.Message}");
            throw;
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"JSON error processing charge: {ex.Message}");
            throw new InvalidOperationException("Failed to process JSON response.", ex);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"General error handling charge: {ex.Message}");
            throw;
        }
    }

    private BaseResponse? TransformJsonRawObject(Dictionary<string, object> obj, string? rawObj, string type = "object")
    {
        BaseResponse? response = null;
        if (rawObj != null)
        {
            if (type == "Subscription")
            {
                var subResponse = JsonConvert.DeserializeObject<SubscriptionResponseData>(rawObj) ?? new SubscriptionResponseData();
                subResponse.RawData = rawObj;
                subResponse.ObjectId ??= $"sub_{obj["id"]}";
                response = subResponse;
            }
        }
       
        return response;
    }
}