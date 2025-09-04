namespace PayarcSDK.Entities;

using Newtonsoft.Json;

public class DepositListResponse : ListBaseResponse {
    [JsonProperty("deposits")]
    public override List<BaseResponse?>? Data { get; set; }
}