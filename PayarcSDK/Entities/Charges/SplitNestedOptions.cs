namespace PayarcSDK.Entities;

using Newtonsoft.Json;
public class SplitNestedOptions
{
    [JsonProperty("mid")]
    public string? Mid { get; set; }
    
    [JsonProperty("percent")]
    public long? Percent { get; set; }
    
    [JsonProperty("amount")]
    public long? Amount { get; set; }
}