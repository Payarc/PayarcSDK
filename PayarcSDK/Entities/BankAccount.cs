using Newtonsoft.Json;

namespace PayarcSDK.Entities;

public class BankAccount : BaseResponse {
	[JsonProperty("object")]
	public override string Object { get; set; }

	[JsonProperty("id")]
	public string Id { get; set; }

	[JsonProperty("first_name")]
	public string FirstName { get; set; }

	[JsonProperty("last_name")]
	public string LastName { get; set; }

	[JsonProperty("company_name")]
	public string CompanyName { get; set; }

	[JsonProperty("account_type")]
	public string AccountType { get; set; }

	[JsonProperty("sec_code")]
	public string SecCode { get; set; }

	[JsonProperty("routing_number")]
	public string RoutingNumber { get; set; }

	[JsonProperty("account_number")]
	public string AccountNumber { get; set; }

	[JsonProperty("is_default")]
	public string IsDefault { get; set; }

	public override string ToString() {
		return JsonConvert.SerializeObject(this);
	}
}

