namespace PayarcSDK.Entities;
using Newtonsoft.Json;

public class Refund
{
    [JsonProperty("object")]
    public string Object { get; set; } = "Refund";

    [JsonProperty("id")]
    public string? Id { get; set; }

    [JsonProperty("refund_amount")]
    public decimal? RefundAmount { get; set; }

    [JsonProperty("currency")]
    public string? Currency { get; set; }

    [JsonProperty("status")]
    public string? Status { get; set; }

    [JsonProperty("reason")]
    public string? Reason { get; set; }

    [JsonProperty("description")]
    public string? Description { get; set; }

    [JsonProperty("email")]
    public string? Email { get; set; }

    [JsonProperty("receipt_phone")]
    public string? ReceiptPhone { get; set; }

    [JsonProperty("charge_id")]
    public string? ChargeId { get; set; }

    [JsonProperty("created_at")]
    public string? CreatedAt { get; set; }

    [JsonProperty("updated_at")]
    public string? UpdatedAt { get; set; }

    [JsonProperty("do_not_send_email_to_customer")]
    public bool? DoNotSendEmailToCustomer { get; set; }

    [JsonProperty("do_not_send_sms_to_customer")]
    public bool? DoNotSendSmsToCustomer { get; set; }

    [JsonProperty("real_id")]
    public string? RealId { get; set; }
}