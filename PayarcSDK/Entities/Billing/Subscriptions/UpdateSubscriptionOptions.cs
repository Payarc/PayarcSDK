using Newtonsoft.Json;

namespace PayarcSDK.Entities.Billing.Subscriptions;

public class UpdateSubscriptionOptions
{
    [JsonProperty("description")]
    public string? Description { get; set; }
    
    [JsonProperty("tax_percent")]
    public double TaxPercent { get; set; }
     
    [JsonProperty("payment_due_days")]
    public int? PaymentDueDays { get; set; } 
    
    [JsonProperty("billing_type")]
    public int? BillingType { get; set; }

    [JsonProperty("end_after_cycles")]
    public int? EndAfterCycle { get; set; }
}