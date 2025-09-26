using Newtonsoft.Json;
using PayarcSDK.Entities.Billing;

namespace PayarcSDK.Entities;

public class ChargeRequestPayload : BaseRequestPayload
{
    [JsonProperty("account_number")]
    public string? AccountNumber { get; set; }
    
    [JsonProperty("routing_number")]
    public string? RoutingNumber { get; set; }
    
    [JsonProperty("first_name")]
    public string? FirstName { get; set; }
    
    [JsonProperty("last_name")]
    public string? LastName { get; set; }
    
    [JsonProperty("account_type")]
    public string? AccountType { get; set; }

    [JsonProperty("amount")]
    public long? Amount { get; set; }

    [JsonProperty("currency")]
    public string? Currency { get; set; }

    [JsonProperty("card_id")]
    public string? CardId { get; set; }

    [JsonProperty("exp_month")]
    public string? ExpMonth { get; set; }

    [JsonProperty("exp_year")]
    public string? ExpYear { get; set; }
    
    [JsonProperty("country")]
    public string? CountyCode { get; set; }
    
    [JsonProperty("state")]
    public string? State { get; set; }
    
    [JsonProperty("city")]
    public string? City { get; set; }
    
    [JsonProperty("address_line1")]
    public string? AddressLine1 { get; set; }
    
    [JsonProperty("zip")]
    public string? ZipCode { get; set; }
    
    [JsonProperty("customer_id")]
    public string? CustomerId { get; set; }
    
    [JsonProperty("object_id")]
    public string? ObjectId { get; set; }
    
    [JsonProperty("token_id")]
    public string? TokenId { get; set; }
    
    [JsonProperty("sec_code")]
    public string? SecCode { get; set; }
    
    [JsonProperty("bank_account_id")]
    public string? BankAccountId { get; set; }

    [JsonProperty("type")]
    public string? Type { get; set; }

    [JsonProperty("card_number")]
    public string? CardNumber { get; set; }

    [JsonProperty("splits")]
    public List<SplitNestedOptions>? Splits { get; set; }
}