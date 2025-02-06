using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace PayarcSDK.Entities
{
    public class CampaignResponseData : BaseResponse {
		[JsonProperty("object")]
		public string Object { get; set; }

		[JsonIgnore]
		public Func<SplitCampaignRequestData?, Task<BaseResponse?>> Update { get; set; }

		[JsonIgnore]
		public Func<Task<BaseResponse?>> Retrieve { get; set; }

		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("description")]
		public string Description { get; set; }

		[JsonProperty("notes")]
		public string Notes { get; set; }

		[JsonProperty("status")]
		public string Status { get; set; }

		[JsonProperty("base_charge")]
		public double BaseCharge { get; set; }

		[JsonProperty("perc_charge")]
		public double PercCharge { get; set; }

		[JsonProperty("is_default")]
		public string IsDefault { get; set; }

		[JsonProperty("created_at")]
		public DateTimeOffset CreatedAt { get; set; }

		[JsonProperty("updated_at")]
		public DateTimeOffset UpdatedAt { get; set; }

		[JsonProperty("readable_created_at")]
		public string ReadableCreatedAt { get; set; }

		[JsonProperty("readable_updated_at")]
		public string ReadableUpdatedAt { get; set; }

		[JsonProperty("account")]
		public Account Account { get; set; }
	}

	public partial class Account {
		[JsonProperty("data")]
		public object[] Data { get; set; }
	}
}
