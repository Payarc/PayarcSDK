using Newtonsoft.Json;

namespace PayarcSDK.Entities;

public class CustomerListResponse : ListBaseResponse {
	[JsonProperty("customers")]
	public override List<BaseResponse?>? Data { get; set; }
}
