using Newtonsoft.Json;

namespace PayarcSDK.Entities.CustomerService;

public class CustomerListResponse : ListBaseResponse {
	[JsonProperty("customers")]
	public override List<BaseResponse?>? Data { get; set; }
}
