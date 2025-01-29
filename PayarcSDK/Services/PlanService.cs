using AnyOfTypes;
using Newtonsoft.Json;
using PayarcSDK.Entities;
using System.Text;
using System.Text.Json;
using JsonException = System.Text.Json.JsonException;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace PayarcSDK.Services;

public class PlanService : CommonServices
{
    private readonly HttpClient _httpClient;

    public PlanService(HttpClient httpClient)
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
        var parameters = new Dictionary<string, object>
        {
            { "limit", options?.Limit ?? 9999 },
            { "page", options?.Page ?? 0 }
        };
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

    private async Task<ListBaseResponse?> GetPlansAsync(string endpoint, string? queryParams)
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
                    var ch = TransformJsonRawObject(jsonPlans[i], JsonSerializer.Serialize(jsonPlans[i]));
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
        PlanRequestPayload? planData = null)
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
                    return TransformJsonRawObject(dataDict, dataElement.GetRawText());
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
    
    private BaseResponse? TransformJsonRawObject(Dictionary<string, object> obj, string? rawObj)
    {
        BaseResponse? response = null;
        if (rawObj != null)
        {
            var planResponse = JsonConvert.DeserializeObject<PlanResponseData>(rawObj) ?? new PlanResponseData();
            planResponse.RawData = rawObj;
            if (obj["plan_id"]?.ToString() != null)
            {
                planResponse.Object = "Plan";
                planResponse.ObjectId ??= $"{obj["plan_id"]}";
                // chargeResponse.CreateRefund = async (chargeData) => await CreateRefund(chargeResponse, chargeData);
            }
            response = planResponse;
        }
        return response;
    }
}