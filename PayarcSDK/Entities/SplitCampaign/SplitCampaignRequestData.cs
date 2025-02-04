using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayarcSDK.Entities.SplitCampaign
{
    public class SplitCampaignRequestData {
		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("base_charge")]
		public long BaseCharge { get; set; }

		[JsonProperty("perc_charge")]
		public string PercCharge { get; set; }

		[JsonProperty("description")]
		public string Description { get; set; }

		[JsonProperty("notes")]
		public string Notes { get; set; }

		[JsonProperty("is_default")]
		public string IsDefault { get; set; }

		[JsonProperty("accounts")]
		public string[] Accounts { get; set; }

		public string ToJson() {
			var settings = new JsonSerializerSettings {
				NullValueHandling = NullValueHandling.Ignore,
				Formatting = Formatting.Indented
			};

			return JsonConvert.SerializeObject(this, settings);
		}
	}
}
