namespace PayarcSDK.Entities;

using Newtonsoft.Json;

public class BatchListResponse : ListBaseResponse {
    [JsonProperty("batches")]
    public override List<BaseResponse?>? Data { get; set; }
}