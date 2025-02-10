using Newtonsoft.Json;
using PayarcSDK.Entities.Billing;

namespace PayarcSDK.Entities;

public class PlanRequestPayload : BaseRequestPayload
{
    [JsonProperty("amount")]
    public int? Amount { get; set; }

    [JsonProperty("plan_type")]
    public string? PlanType { get; set; }

    [JsonProperty("name")]
    public string? Name { get; set; }

    [JsonProperty("interval")]
    public string? Interval { get; set; }

    [JsonProperty("statement_descriptor")]
    public string? StatementDescriptor { get; set; }

    [JsonProperty("interval_count")]
    public int? IntervalCount { get; set; }

    [JsonProperty("trial_period_days")]
    public int? TrialPeriodDays { get; set; }

    [JsonProperty("plan_id")]
    public string? PlanId { get; set; }

    [JsonProperty("plan_description")]
    public string? PlanDescription { get; set; }

    [JsonProperty("currency")]
    public string? Currency { get; set; }

    [JsonProperty("surcharge_applicable")]
    public int? SurchargeApplicable { get; set; }
    
}