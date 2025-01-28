using Newtonsoft.Json;

namespace PayarcSDK.Models
{
    public class CardData {
		[JsonProperty("card_source")]
		public string CardSource { get; set; }

		[JsonProperty("card_number")]
		public long CardNumber { get; set; }

		[JsonProperty("exp_month")]
		public int ExpMonth { get; set; }

		[JsonProperty("exp_year")]
		public int ExpYear { get; set; }

		[JsonProperty("cvv")]
		public int Cvv { get; set; }

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
		public int Zip { get; set; }

		[JsonProperty("country")]
		public string Country { get; set; }

		[JsonProperty("authorize_card")]
		public int AuthorizeCard { get; set; }

		public string ToJson() {
			var settings = new JsonSerializerSettings {
				NullValueHandling = NullValueHandling.Ignore,
				Formatting = Formatting.Indented
			};

			return JsonConvert.SerializeObject(this, settings);
		}
	}
}
