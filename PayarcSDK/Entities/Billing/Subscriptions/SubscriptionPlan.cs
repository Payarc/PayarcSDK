using Newtonsoft.Json;

namespace PayarcSDK.Entities.Billing.Subscriptions;

public class SubscriptionPlan : BaseResponse
{
    [JsonProperty("object")]
    public override string? Object { get; set; }
    [JsonIgnore]
    [JsonProperty("object_id")]
    public override string? ObjectId { get; set; }
    [JsonIgnore]
    [JsonProperty("id")]
    public override string? Id { get; set; }
    [JsonProperty("plan_id")]
    public string? PlanId { get; set; }
    [JsonProperty("amount")]
    public string? Amount { get; set; }

    [JsonProperty("interval")]
    public string? Interval { get; set; }

    [JsonProperty("interval_count")]
    public string? IntervalCount { get; set; }

    [JsonProperty("name")]
    public string? Name { get; set; }

    [JsonProperty("plan_description")]
    public string? PlanDescription { get; set; }

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
}