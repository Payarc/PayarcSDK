using AnyOfTypes;
using Newtonsoft.Json;
using PayarcSDK.Entities.Billing.Subscriptions;

namespace PayarcSDK.Entities;

public class PlanResponseData : BaseResponse
{
    [JsonProperty("object")]
    public override string? Object { get; set; }
    
    [JsonIgnore]
    [JsonProperty("id")]
    public override string? Id { get; set; }

    [JsonProperty("amount")]
    public string? Amount { get; set; }

    [JsonProperty("interval")]
    public string? Interval { get; set; }

    [JsonProperty("interval_count")]
    public string? IntervalCount { get; set; }

    [JsonProperty("name")]
    public string? Name { get; set; }

    [JsonProperty("description")]
    public string? Description { get; set; }

    [JsonProperty("statement_descriptor")]
    public string? StatementDescriptor { get; set; }

    [JsonProperty("trial_period_days")]
    public string? TrialPeriodDays { get; set; }

    [JsonProperty("currency")]
    public string? Currency { get; set; }

    [JsonProperty("created_at")]
    public string? CreatedAt { get; set; }

    [JsonProperty("updated_at")]
    public string? UpdatedAt { get; set; }
    
    [JsonIgnore]
    public Func<Task<BaseResponse?>> Retrieve { get; set; }
    [JsonIgnore]
    public Func<UpdatePlanOptions?, Task<BaseResponse?>> Update { get; set; }
    
    [JsonIgnore]
    public Func<Task<BaseResponse?>> Delete { get; set; }
    
    [JsonIgnore]
    public  Func<SubscriptionCreateOptions?, Task<BaseResponse?>> CreateSubscription { get; set; }
}
