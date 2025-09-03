namespace PayarcSDK.Entities;

using Newtonsoft.Json;

public class BatchDetailResponse : ListBaseResponse {
	[JsonProperty("batch_detail")]
	public override List<BaseResponse?>? Data { get; set; }
}