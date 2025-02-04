using Newtonsoft.Json;
using PayarcSDK.Entities;

namespace PayarcSDK.Entities
{
    public class CustomerRequestData {
		[JsonProperty("email")]
		public string Email { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("description")]
		public string Description { get; set; }

		[JsonProperty("send_email_address")]
		public string SendEmailAddress { get; set; }

		[JsonProperty("cc_email_address")]
		public string CcEmailAddress { get; set; }

		[JsonProperty("country")]
		public string Country { get; set; }

		[JsonProperty("address_1")]
		public string Address1 { get; set; }

		[JsonProperty("address_2")]
		public string Address2 { get; set; }

		[JsonProperty("city")]
		public string City { get; set; }

		[JsonProperty("state")]
		public string State { get; set; }

		[JsonProperty("zip")]
		public int? Zip { get; set; }

		[JsonProperty("phone")]
		public long Phone { get; set; }

		[JsonProperty("token_id")]
		public string TokenId { get; set; }

		[JsonProperty("default_card_id")]
		public string DefaultCardId { get; set; }

		[JsonProperty("cards")]
		public List<CardData>? Cards { get; set; }

		[JsonProperty("bank_accounts")]
		public List<BankData>? BankAccounts { get; set; }

		public string ToJson() {
			var settings = new JsonSerializerSettings {
				NullValueHandling = NullValueHandling.Ignore,
				Formatting = Formatting.Indented
			};

			return JsonConvert.SerializeObject(this, settings);
		}
	}
}
