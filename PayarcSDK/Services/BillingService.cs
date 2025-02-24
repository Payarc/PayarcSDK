using AnyOfTypes;

namespace PayarcSDK.Services;


public class BillingService
{
    private readonly HttpClient _httpClient;

    public BillingService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        Plan = new PlanService(_httpClient);
        Subscription = new SubscriptionService(_httpClient);
    }
    
    public PlanService Plan { get; set; }
    public SubscriptionService Subscription { get; set; }
}