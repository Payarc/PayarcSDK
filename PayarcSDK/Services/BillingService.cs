using AnyOfTypes;
using PayarcSDK.Http;

namespace PayarcSDK.Services;


public class BillingService
{
    private readonly ApiClient _apiClient;
    private readonly HttpClient _httpClient;

    public BillingService(AnyOf<ApiClient, HttpClient> apiClient)
    {
        _apiClient = apiClient.IsFirst ? apiClient.First : new ApiClient(apiClient.Second);
        _httpClient = apiClient.IsSecond ? apiClient.Second : new HttpClient();
        Plan = new PlanService(apiClient);
    }
    
    public PlanService Plan { get; set; }
}