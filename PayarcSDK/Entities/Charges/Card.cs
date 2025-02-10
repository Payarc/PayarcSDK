using Newtonsoft.Json;

namespace PayarcSDK.Entities;

public class Card : BaseResponse {
	[JsonProperty("object")]
	public override string? Object { get; set; }

	[JsonProperty("id")]
	public string? Id { get; set; }

	[JsonProperty("address1")]
	public string? Address1 { get; set; }

	[JsonProperty("address2")]
	public string? Address2 { get; set; }

	[JsonProperty("card_source")]
	public string? CardSource { get; set; }

	[JsonProperty("card_holder_name")]
	public string? CardHolderName { get; set; }

	[JsonProperty("is_default")]
	public bool? IsDefault { get; set; }

	[JsonProperty("exp_month")]
	public string? ExpMonth { get; set; }

	[JsonProperty("exp_year")]
	public string? ExpYear { get; set; }

	[JsonProperty("is_verified")]
	public string? IsVerified { get; set; }

	[JsonProperty("fingerprint")]
	public string? Fingerprint { get; set; }

	[JsonProperty("city")]
	public string? City { get; set; }

	[JsonProperty("state")]
	public string? State { get; set; }

	[JsonProperty("zip")]
	public string? Zip { get; set; }

	[JsonProperty("brand")]
	public string? Brand { get; set; }

	[JsonProperty("last4digit")]
	public string? Last4Digit { get; set; }

	[JsonProperty("first6digit")]
	public string? First6Digit { get; set; }

	[JsonProperty("country")]
	public string? Country { get; set; }

	[JsonProperty("avs_status")]
	public string? AvsStatus { get; set; }

	[JsonProperty("cvc_status")]
	public string? CvcStatus { get; set; }

	[JsonProperty("address_check_passed")]
	public string? AddressCheckPassed { get; set; }

	[JsonProperty("zip_check_passed")]
	public string? ZipCheckPassed { get; set; }

	[JsonProperty("customer_id")]
	public string? CustomerId { get; set; }

	[JsonProperty("created_at")]
	public string? CreatedAt { get; set; }

	[JsonProperty("updated_at")]
	public string? UpdatedAt { get; set; }

	[JsonProperty("card_type")]
	public string? CardType { get; set; }

	[JsonProperty("bin_country")]
	public string? BinCountry { get; set; }

	[JsonProperty("bank_name")]
	public string? BankName { get; set; }

	[JsonProperty("bank_website")]
	public string? BankWebsite { get; set; }

	[JsonProperty("bank_phone")]
	public string? BankPhone { get; set; }

	public override string ToString() {
		return JsonConvert.SerializeObject(this);
	}
}

