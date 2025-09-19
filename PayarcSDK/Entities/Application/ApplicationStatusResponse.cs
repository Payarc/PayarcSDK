using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayarcSDK.Entities
{
    public class ApplicationStatusResponse : BaseResponse {
		[JsonProperty("object")]
		public override string? Object { get; set; }

		[JsonProperty("object_id")]
		public override string? ObjectId { get; set; }

		[JsonIgnore]
		[JsonProperty("MerchantCode")]
		public string? MerchantCode { get; set; }

		[JsonProperty("Status")]
		public int Status { get; set; }

		[JsonProperty("StatusDescription")]
		public string StatusDescription { get; set; }

		[JsonProperty("LeadCode")]
		public string LeadCode { get; set; }

		[JsonProperty("LeadStatus")]
		public string LeadStatus { get; set; }

		[JsonProperty("LeadModule")]
		public string LeadModule { get; set; }

		[JsonProperty("AccountCode")]
		public string AccountCode { get; set; }

		[JsonProperty("AccountMID")]
		public string AccountMID { get; set; }

		[JsonProperty("AccountStatus")]
		public string AccountStatus { get; set; }

		[JsonProperty("AchStatus")]
		public string AchStatus { get; set; }
	}
}
