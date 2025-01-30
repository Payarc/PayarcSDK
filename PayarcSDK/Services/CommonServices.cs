using System.Reflection;
using AnyOfTypes;
using Newtonsoft.Json;
using PayarcSDK.Entities;
using PayarcSDK.Entities.Billing.Subscriptions;
using PayarcSDK.Http;

namespace PayarcSDK.Services;

public class CommonServices
{
    private readonly ApiClient _apiClient;
    private readonly HttpClient _httpClient;

    public CommonServices(AnyOf<ApiClient, HttpClient> apiClient)
    {
        _apiClient = apiClient.IsFirst ? apiClient.First : new ApiClient(apiClient.Second);
        _httpClient = apiClient.IsSecond ? apiClient.Second : new HttpClient();
    }
    private CommonServices GetBaseInstance()
    {
        return (CommonServices)this.GetType().BaseType.GetConstructor(new Type[] { }).Invoke(null);
    }
    public string BuildQueryString(Dictionary<string, object?> parameters)
    {
        var queryString = string.Join("&",
            parameters.Select(p =>
                $"{Uri.EscapeDataString(p.Key)}={Uri.EscapeDataString(p.Value?.ToString() ?? string.Empty)}"));
        return queryString;
    }
    
    public BaseResponse? TransformJsonRawObject(Dictionary<string, object> obj, string? rawObj, string type = "object")
    {
        BaseResponse? response = null;
        if (rawObj != null)
        {
            if (type == "Plan")
            {
                var planResponse = JsonConvert.DeserializeObject<PlanResponseData>(rawObj) ?? new PlanResponseData();
                planResponse.RawData = rawObj;
                if (obj.ContainsKey("plan_id") && obj["plan_id"]?.ToString() != null)
                {
                    var planService = new PlanService(_httpClient);
                    planResponse.Object = "Plan";
                    planResponse.ObjectId ??= $"{obj["plan_id"]}";
                    planResponse.Retrieve = async() => await planService.Retrieve(planResponse);
                    planResponse.Update = async (newData) => await planService.Update(planResponse, newData);
                    planResponse.Delete = async() => await planService.Delete(planResponse);
                    planResponse.CreateSubscription = async (newData) => await planService.CreateSubscription(planResponse, newData);
                }
                response = planResponse;
            }
            if (type == "Subscription")
            {
                var subService = new SubscriptionService(_httpClient);
                var subResponse = JsonConvert.DeserializeObject<SubscriptionResponseData>(rawObj) ?? new SubscriptionResponseData();
                subResponse.RawData = rawObj;
                subResponse.ObjectId ??= $"sub_{obj["id"]}";
                subResponse.Update = async (newData) => await subService.Update(subResponse, newData);
                subResponse.Cancel = async () => await subService.Cancel(subResponse);
                response = subResponse;
            }
        }
       
        return response;
    }
    
    
}