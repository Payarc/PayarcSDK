using Newtonsoft.Json;

namespace PayarcSDK.Entities;

public abstract class BaseResponse
{
    [JsonProperty("object")]
    public string? Object { get; set; }
    [JsonProperty("id")]
    public string? Id { get; set; }
    [JsonIgnore]
    public string? RawData { get; set; }
        
    public override string ToString()
    {
        var settings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented
        };
    
        return JsonConvert.SerializeObject(this, settings);
    }
}