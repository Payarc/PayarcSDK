using Newtonsoft.Json;

namespace PayarcSDK.Entities.Billing;

public class BaseRequestPayload
{
    [JsonIgnore]
    public Dictionary<string, object>? Parameters { get; set; }
    public string ToJson()
    {
        var settings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            Formatting = Formatting.Indented
        };
    
        return JsonConvert.SerializeObject(this, settings);
    }
}