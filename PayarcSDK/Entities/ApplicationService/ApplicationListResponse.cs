using Newtonsoft.Json;

namespace PayarcSDK.Entities.ApplicationService;

public class ApplicationListResponse : ListBaseResponse {
	[JsonProperty("applications")]
	public override List<BaseResponse?>? Data { get; set; }
}