namespace PayarcSDK.Entities;

using Newtonsoft.Json;

public abstract class ListBaseResponse
{
    
    [JsonProperty("data")]
    public virtual List<BaseResponse?>? Data { get; set; }
    
    [JsonProperty("pagination")]
    public virtual Dictionary<string, object>? Pagination { get; set; }

	[JsonIgnore]
	public string? RawData { get; set; }
        
    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }
}