namespace PayarcSDK.Entities;

using Newtonsoft.Json;
using AnyOfTypes;



public class ChargeCreateOptions
{
    [JsonProperty("amount")]
    public long? Amount { get; set; }
    
    [JsonProperty("currency")]
    public string Currency { get; set; }
    
    [JsonProperty("object_id")]
    public string? ObjectId { get; set; }
    
    [JsonProperty("token_id")]
    public string? TokenId { get; set; }
    
    [JsonProperty("customer_id")]
    public string? CustomerId { get; set; }
    
    [JsonProperty("card_id")]
    public string? CardId { get; set; }
    [JsonProperty("sec_code")]
    public string? SecCode { get; set; }
    [JsonProperty("bank_account_id")]
    public string? BankAccountId { get; set; }
    
    [JsonProperty("source")]
    [JsonConverter(typeof(AnyOfConverter<,>))]
    public AnyOf<string, CardCreateNestedOptions>? Source { get; set; }
}