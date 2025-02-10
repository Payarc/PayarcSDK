using Newtonsoft.Json;

namespace PayarcSDK.Entities;

public class PlanListResponse : ListBaseResponse
{
    [JsonProperty("plans")]
    public override List<BaseResponse?>? Data { get; set; }
}