using Newtonsoft.Json;

namespace PayarcSDK.Entities
{
    public class BankData {
		[JsonProperty("account_number")]
		public string AccountNumber { get; set; }

		[JsonProperty("routing_number")]
		public string RoutingNumber { get; set; }

		[JsonProperty("first_name")]
		public string FirstName { get; set; }

		[JsonProperty("last_name")]
		public string LastName { get; set; }

		[JsonProperty("account_type")]
		public string AccountType { get; set; }

		[JsonProperty("sec_code")]
		public string SecCode { get; set; }

		[JsonProperty("customer_id")]
		public string CustomerId { get; set; }

		public string ToJson() {
			var settings = new JsonSerializerSettings {
				NullValueHandling = NullValueHandling.Ignore,
				Formatting = Formatting.Indented
			};

			return JsonConvert.SerializeObject(this, settings);
		}
	}
}
