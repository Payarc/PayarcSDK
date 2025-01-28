using AnyOfTypes;
using PayarcSDK.Http;

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
}