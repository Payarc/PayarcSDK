using System.Net;
using AnyOfTypes;
using Newtonsoft.Json;
using PayarcSDK.Entities;
using PayarcSDK.Http;
using System.Text;
using System.Text.Json;
using PayarcSDK.Entities.Billing;
using PayarcSDK.Entities.Billing.Subscriptions;
using JsonException = System.Text.Json.JsonException;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace PayarcSDK.Services;

public class PlanService : CommonServices
{
    private readonly HttpClient _httpClient;

    public PlanService(HttpClient httpClient) : base(httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<BaseResponse?> Create(PlanCreateOptions options)
    {
        try
        {
            var planData = new PlanRequestPayload();
            JsonConvert.PopulateObject(JsonConvert.SerializeObject(options), planData);
            planData.Currency ??= "usd";
            planData.PlanType ??= "digital";
            return await HandlePlanAsync(HttpMethod.Post, "plans", planData);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }

    public async Task<BaseResponse?> Retrieve(AnyOf<string, PlanResponseData> plan)
    {
        try
        {
            var planId = plan.IsSecond ? plan.Second.ObjectId : (plan.IsFirst ? plan.First : null);
            return await HandlePlanAsync(HttpMethod.Get, $"plans/{planId}");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }

    public async Task<ListBaseResponse?> List(PlanListOptions? options = null)
    {
        
        var parameters = new Dictionary<string, object?>
            {
                { "limit", options?.Limit ?? 9999 },
                { "page", options?.Page ?? 1 },
                { "search", options?.Search }
            }.Where(kvp => kvp.Value != null)
            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        var query = BuildQueryString(parameters);
        return await GetPlansAsync("plans", query);
    }

    public async Task<BaseResponse?> Update(AnyOf<string, PlanResponseData> plan, UpdatePlanOptions? options = null)
    {
        try
        {
            var planId = plan.IsSecond ? plan.Second.ObjectId : (plan.IsFirst ? plan.First : null);
            var planData = new PlanRequestPayload();
            JsonConvert.PopulateObject(JsonConvert.SerializeObject(options), planData);
            return await HandlePlanAsync(HttpMethod.Patch, $"plans/{planId}", planData);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }

    public async Task<BaseResponse?> CreateSubscription(AnyOf<string, PlanResponseData> plan,
        SubscriptionCreateOptions? options = null)
    {
        try
        {
            var planId = plan.IsSecond ? plan.Second.ObjectId : (plan.IsFirst ? plan.First : null);
            var subData = new SubscriptionRequestPeyload();
            JsonConvert.PopulateObject(JsonConvert.SerializeObject(options), subData);
            subData.PlanId ??= planId;
            subData.CustomerId = subData.CustomerId.StartsWith("cus_") ?
                                 subData.CustomerId.Substring("cus_".Length) : subData.CustomerId;
            return await HandlePlanAsync(HttpMethod.Post, "subscriptions", subData, "Subscription");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<BaseResponse?> Delete(AnyOf<string, PlanResponseData> plan)
    {
        try
        {
            var planId = plan.IsSecond ? plan.Second.ObjectId : (plan.IsFirst ? plan.First : null);
            return await HandlePlanAsync(HttpMethod.Delete, $"plans/{planId}");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    private string BuildQueryString(Dictionary<string, object?> parameters)
    {
        var queryString = string.Join("&",
            parameters.Select(p =>
                $"{Uri.EscapeDataString(p.Key)}={Uri.EscapeDataString(p.Value.ToString() ?? string.Empty)}"));
        return queryString;
    }

    private async Task<ListBaseResponse?> GetPlansAsync(string endpoint, string? queryParams, string objectType = "Plan")
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
            var jsonPlans = dataElement.Deserialize<List<Dictionary<string, object>>>();
            List<BaseResponse?>? plans = new List<BaseResponse?>();
            if (jsonPlans != null)
            {
                for (int i = 0; i < jsonPlans.Count; i++)
                {
                    var ch = TransformJsonRawObject(jsonPlans[i], JsonSerializer.Serialize(jsonPlans[i]), objectType);
                    plans?.Add(ch);
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
            return new PlanListResponse
            {
                Data = plans,
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
    public async Task<BaseResponse?> HandlePlanAsync(HttpMethod method, string path,
        BaseRequestPayload? planData = null, string objectType = "Plan")
    {
        try
        {
            HttpContent? content  = null;
            if (planData != null)
            {
                if (method == HttpMethod.Post && planData.Parameters != null)
                {
                    content = new StringContent(JsonConvert.SerializeObject(planData.Parameters), Encoding.UTF8,
                        "application/json");
                    Console.WriteLine(JsonConvert.SerializeObject(planData.Parameters));
                }
                else
                {
                    content = new StringContent(planData.ToJson(), Encoding.UTF8, "application/json");
                    Console.WriteLine(planData.ToJson());
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

    private BaseResponse? TransformJsonRawObject(Dictionary<string, object> obj, string? rawObj, string type = "object")
    {
        return base.TransformJsonRawObject(obj, rawObj, type);
    }
}