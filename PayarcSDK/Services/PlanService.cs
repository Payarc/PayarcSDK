using AnyOfTypes;
using PayarcSDK.Http;

namespace PayarcSDK.Services;

public class PlanService
{
    private readonly ApiClient _apiClient;
    private readonly HttpClient _httpClient;

    public PlanService(AnyOf<ApiClient, HttpClient> apiClient)
    {
        _apiClient = apiClient.IsFirst ? apiClient.First : new ApiClient(apiClient.Second);
        _httpClient = apiClient.IsSecond ? apiClient.Second : new HttpClient();
    }

    public void Create()
    {
        Console.WriteLine("Create Plan");
    }
}