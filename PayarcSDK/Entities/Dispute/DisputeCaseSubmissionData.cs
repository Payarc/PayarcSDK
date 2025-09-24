using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayarcSDK.Entities.Dispute
{
    public class DisputeCaseSubmissionData : BaseResponse {
		[JsonProperty("object")]
		public override string Object { get; set; }

        [JsonProperty("object_id")]
        public override string? ObjectId
        {
            get
            {
                if (string.IsNullOrEmpty(Object) || string.IsNullOrEmpty(Id))
                    return null;
                return $"sbm_{Id}";
            }
        }

        [JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("description")]
		public string Description { get; set; }

		[JsonProperty("fax_id")]
		public string FaxId { get; set; }

		[JsonProperty("destination_fax_number")]
		public string DestinationFaxNumber { get; set; }

		[JsonProperty("errors")]
		public string Errors { get; set; }

		[JsonProperty("case_id")]
		public long CaseId { get; set; }

		[JsonProperty("dispute_id")]
		public object DisputeId { get; set; }

		[JsonProperty("created_at")]
		public string CreatedAt { get; set; }

		[JsonProperty("updated_at")]
		public string UpdatedAt { get; set; }

		[JsonProperty("success")]
		public long Success { get; set; }
	}
}
