using Newtonsoft.Json;

namespace PayarcSDK.Entities
{
    public class CardData {
		[JsonProperty("card_source")]
		public string CardSource { get; set; }

		[JsonProperty("card_number")]
		public string CardNumber { get; set; }

		[JsonProperty("exp_month")]
		public string ExpMonth { get; set; }

		[JsonProperty("exp_year")]
		public string ExpYear { get; set; }

		[JsonProperty("cvv")]
		public string Cvv { get; set; }

		[JsonProperty("card_holder_name")]
		public string CardHolderName { get; set; }

		[JsonProperty("address_line1")]
		public string AddressLine1 { get; set; }

		[JsonProperty("address_line2")]
		public string AddressLine2 { get; set; }

		[JsonProperty("city")]
		public string City { get; set; }

		[JsonProperty("state")]
		public string State { get; set; }

		[JsonProperty("zip")]
		public string Zip { get; set; }

		[JsonProperty("country")]
		public string Country { get; set; }

		[JsonProperty("authorize_card")]
		public string AuthorizeCard { get; set; }

		public string ToJson() {
			var settings = new JsonSerializerSettings {
				NullValueHandling = NullValueHandling.Ignore,
				Formatting = Formatting.Indented
			};

			return JsonConvert.SerializeObject(this, settings);
		}
	}
}
