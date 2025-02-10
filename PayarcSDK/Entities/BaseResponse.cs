using Newtonsoft.Json;

namespace PayarcSDK.Entities;

public abstract class BaseResponse
{
    [JsonProperty("object")]
    public virtual string? Object { get; set; }
    
    [JsonProperty("object_id")]
    public virtual string? ObjectId { get; set; }
    
    [JsonProperty("id")]
    public virtual string? Id { get; set; }

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