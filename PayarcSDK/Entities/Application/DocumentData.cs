using Newtonsoft.Json;

namespace PayarcSDK.Entities
{
	public class DocumentData {
		[JsonProperty("MerchantCode")]
		public string MerchantCode { get; set; }

		[JsonProperty("MerchantDocuments")]
		public List<MerchantDocument> MerchantDocuments { get; set; }

		public string ToJson() {
			var settings = new JsonSerializerSettings {
				NullValueHandling = NullValueHandling.Ignore,
				Formatting = Formatting.Indented
			};

			return JsonConvert.SerializeObject(this, settings);
		}
	}
}
