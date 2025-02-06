using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayarcSDK.Entities
{
    public class DisputeEvidenceData : BaseResponse {
		[JsonProperty("object")]
		public override string Object { get; set; }

		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("text")]
		public string Text { get; set; }

		[JsonProperty("file_name")]
		public string FileName { get; set; }

		[JsonProperty("file_size")]
		public long FileSize { get; set; }

		[JsonProperty("created_at")]
		public long CreatedAt { get; set; }

		[JsonProperty("updated_at")]
		public long UpdatedAt { get; set; }

		[JsonProperty("file_type")]
		public string FileType { get; set; }
	}
}
