using Newtonsoft.Json;

namespace PayarcSDK.Entities.Billing.Subscriptions;

public class SubscriptionResponseData : BaseResponse
{
    [JsonProperty("object")]
    public string? ObjectType { get; set; }

    [JsonProperty("id")]
    public string? Id { get; set; }

    [JsonProperty("customer_id")]
    public string? CustomerId { get; set; }

    [JsonProperty("application_fee_percent")]
    public string? ApplicationFeePercent { get; set; }

    [JsonProperty("billing_type")]
    public int? BillingType { get; set; }

    [JsonProperty("payment_due_days")]
    public int? PaymentDueDays { get; set; }

    [JsonProperty("cancel_at_period_end")]
    public bool? CancelAtPeriodEnd { get; set; }

    [JsonProperty("canceled_at")]
    public string? CanceledAt { get; set; }

    [JsonProperty("current_period_end")]
    public string? CurrentPeriodEnd { get; set; }

    [JsonProperty("current_period_start")]
    public string? CurrentPeriodStart { get; set; }

    [JsonProperty("days_until_due")]
    public int? DaysUntilDue { get; set; }

    [JsonProperty("plan_ref")]
    public string? PlanRef { get; set; }

    [JsonProperty("start_at")]
    public string? StartAt { get; set; }

    [JsonProperty("end_at")]
    public string? EndAt { get; set; }

    [JsonProperty("status")]
    public string? Status { get; set; }

    [JsonProperty("tax_percent")]
    public string? TaxPercent { get; set; }

    [JsonProperty("trial_end")]
    public string? TrialEnd { get; set; }

    [JsonProperty("trial_days")]
    public int? TrialDays { get; set; }

    [JsonProperty("trial_start")]
    public string? TrialStart { get; set; }

    [JsonProperty("created_at")]
    public string? CreatedAt { get; set; }

    [JsonProperty("updated_at")]
    public string? UpdatedAt { get; set; }

    [JsonProperty("description")]
    public string? Description { get; set; }

    [JsonProperty("payment_type")]
    public string? PaymentType { get; set; }
    [JsonProperty("plan")]
    public PlanWrapper? PlanWrapper { get; set; }
}

public class PlanWrapper
{
    [JsonProperty("data")]
    public SubscriptionPlan? Plan { get; set; }
}