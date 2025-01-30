namespace PayarcSDK.Entities;

using Newtonsoft.Json;

public class ChargeListResponse : ListBaseResponse
{
    [JsonProperty("charges")]
    public override List<BaseResponse?>? Data { get; set; }
}