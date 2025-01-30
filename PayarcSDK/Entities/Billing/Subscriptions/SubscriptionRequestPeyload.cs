using Newtonsoft.Json;

namespace PayarcSDK.Entities.Billing.Subscriptions;

public class SubscriptionRequestPeyload : BaseRequestPayload
{
    [JsonProperty("plan_id")]
    public string? PlanId { get; set; }

    [JsonProperty("customer_id")]
    public string CustomerId { get; set; }

    [JsonProperty("description")]
    public string? Description { get; set; }

    [JsonProperty("start_after_days")]
    public int? StartAfterDays { get; set; } 
     
    [JsonProperty("payment_due_days")]
    public int? PaymentDueDays { get; set; } 
    
    [JsonProperty("billing_type")]
    public int? BillingType { get; set; }

    [JsonProperty("trial_days")]
    public int? TrialDays { get; set; }
    
    [JsonProperty("discount_id")]
    public string? DiscountId { get; set; }

    [JsonProperty("end_after_cycles")]
    public int? EndAfterCycle { get; set; }
}