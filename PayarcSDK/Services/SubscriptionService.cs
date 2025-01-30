using System.Net;
using System.Text;
using System.Text.Json;
using AnyOfTypes;
using Newtonsoft.Json;
using PayarcSDK.Entities;
using PayarcSDK.Entities.Billing;
using PayarcSDK.Entities.Billing.Subscriptions;
using PayarcSDK.Http;
using JsonException = System.Text.Json.JsonException;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace PayarcSDK.Services;

public class SubscriptionService : CommonServices
{
    private readonly ApiClient _apiClient;
    private readonly HttpClient _httpClient;

    public SubscriptionService(AnyOf<ApiClient, HttpClient> apiClient) : base(apiClient)
    {
        _apiClient = apiClient.IsFirst ? apiClient.First : new ApiClient(apiClient.Second);
        _httpClient = apiClient.IsSecond ? apiClient.Second : new HttpClient();
    }

    public async Task<BaseResponse?> Update(AnyOf<string, SubscriptionResponseData> sub, UpdateSubscriptionOptions? options = null)
    {
        try
        {
            var subId = sub.IsSecond ? sub.Second.ObjectId : (sub.IsFirst ? sub.First : null);
            subId = subId.StartsWith("sub_") ? subId.Substring("sub_".Length) : subId;
            var subData = new SubscriptionRequestPeyload();
            JsonConvert.PopulateObject(JsonConvert.SerializeObject(options), subData);
            return await HandleSubscriptionAsync(HttpMethod.Patch, $"subscriptions/{subId}", subData);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }

    public async Task<BaseResponse?> Cancel(AnyOf<string, SubscriptionResponseData> sub)
    {
        try
        {
            var subId = sub.IsSecond ? sub.Second.ObjectId : (sub.IsFirst ? sub.First : null);
            subId = subId.StartsWith("sub_") ? subId.Substring("sub_".Length) : subId;
            return await HandleSubscriptionAsync(HttpMethod.Patch, $"subscriptions/{subId}/cancel");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
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
    
      public async Task<BaseResponse?> HandleSubscriptionAsync(HttpMethod method, string path,
        BaseRequestPayload? Data = null, string objectType = "Subscription")
    {
        try
        {
            HttpContent? content  = null;
            if (Data != null)
            {
                if (method == HttpMethod.Post && Data.Parameters != null)
                {
                    content = new StringContent(JsonConvert.SerializeObject(Data.Parameters), Encoding.UTF8,
                        "application/json");
                    Console.WriteLine(JsonConvert.SerializeObject(Data.Parameters));
                }
                else
                {
                    content = new StringContent(Data.ToJson(), Encoding.UTF8, "application/json");
                    Console.WriteLine(Data.ToJson());
                }
            }
           
            var request = new HttpRequestMessage(method, path)
            {
                Content = content
            };
            var response = await _httpClient.SendAsync(request);
            var responseBody = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Response status code: {response.StatusCode}");
            // Console.WriteLine($"Response body: {responseBody}");
            if (!response.IsSuccessStatusCode)
            {
                try
                {
                    var errorData = JsonSerializer.Deserialize<Dictionary<string, object>>(responseBody);
                    Console.WriteLine($"Error details: {JsonSerializer.Serialize(errorData)}");
                    throw new InvalidOperationException($"HTTP error {response.StatusCode}: {responseBody}");
                }
                catch (JsonException jsonEx)
                {
                    Console.WriteLine($"Failed to parse error JSON: {jsonEx.Message}");
                    throw new InvalidOperationException(
                        $"HTTP error {response.StatusCode}: Unable to parse error response.");
                }
            }
            if(response.StatusCode == (HttpStatusCode)204 && method == HttpMethod.Delete)
            {
                return null;
            }
            if (string.IsNullOrWhiteSpace(responseBody))
            {
                throw new InvalidOperationException("Response body is empty.");
            }
            var data = JsonSerializer.Deserialize<Dictionary<string, object>>(responseBody);
            if (data != null && data.TryGetValue("data", out var dataValue) && dataValue is JsonElement dataElement)
            {
                var dataDict = JsonSerializer.Deserialize<Dictionary<string, object>>(dataElement.GetRawText());
                if (dataDict != null)
                {
                    return TransformJsonRawObject(dataDict, dataElement.GetRawText(), objectType);
                }
            }
            throw new InvalidOperationException("Response data is invalid or missing.");
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"HTTP error processing charge: {ex.Message}");
            throw;
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"JSON error processing charge: {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"General error handling charge: {ex.Message}");
            throw;
        }
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
        return base.TransformJsonRawObject(obj, rawObj, type);
    }

  
}