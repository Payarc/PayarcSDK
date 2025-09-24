using Newtonsoft.Json;
using PayarcSDK.Helpers;

namespace PayarcSDK.Entities;

public class AchChargeResponseData : BaseResponse
{
    [JsonProperty("object")]
    public override string? Object { get; set; }

    [JsonProperty("object_id")]
    public override string? ObjectId
    {
        get
        {
            if (string.IsNullOrEmpty(Object) || string.IsNullOrEmpty(Id))
                return null;
            return $"ach_{Id}";
        }
    }

    [JsonIgnore]
    public Func< Dictionary<string, object>?, Task<BaseResponse?>> CreateRefund { get; set; }

    [JsonProperty("id")]
    public override string? Id { get; set; }

    [JsonProperty("amount")]
    public decimal? Amount { get; set; }

    [JsonProperty("created_by")]
    public string? CreatedBy { get; set; }

    [JsonProperty("status")]
    public string? Status { get; set; }

    [JsonProperty("type")]
    public string? Type { get; set; }

    [JsonProperty("authorization_id")]
    public string? AuthorizationId { get; set; }

    [JsonProperty("validation_code")]
    public string? ValidationCode { get; set; }

    [JsonProperty("successful")]
    public bool? Successful { get; set; }

    [JsonProperty("response_message")]
    public string? ResponseMessage { get; set; }

    [JsonProperty("created_at")]
    public DateTime? CreatedAt { get; set; }

    [JsonProperty("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    [JsonProperty("retried_achcharge_id")]
    public string? RetriedAchChargeId { get; set; }

    [JsonProperty("sec_code")]
    public string? SecCode { get; set; }
    
    [JsonProperty("bank_account")]
    public BankWrapper? BankAccount { get; set; }

    // [JsonProperty("customer")]
    // public CustomerResponse? Customer { get; set; }
    //
    // [JsonProperty("transaction_metadata")]
    // public List<TransactionMetadataResponse>? TransactionMetadata { get; set; }
}

public class BankWrapper
{
    [JsonProperty("data")]
    [JsonConverter(typeof(BankAccountResponseConverter))] // Apply custom converter
    public BankAccountResponse? Data { get; set; } = null;
}