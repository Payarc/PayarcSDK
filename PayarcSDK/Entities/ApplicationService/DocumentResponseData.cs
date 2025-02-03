using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayarcSDK.Entities.ApplicationService
{
    public class DocumentResponseData : BaseResponse {
		[JsonProperty("object")]
		public string Object { get; set; }

		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("file_name")]
		public string FileName { get; set; }

		[JsonProperty("file_type")]
		public string FileType { get; set; }

		[JsonProperty("file_order")]
		public long FileOrder { get; set; }

		[JsonProperty("created_at")]
		public DateTimeOffset CreatedAt { get; set; }

		[JsonProperty("updated_at")]
		public DateTimeOffset UpdatedAt { get; set; }
	}
}
