using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayarcSDK.Entities
{
    public class DisputeCaseFileData : BaseResponse {
		[JsonProperty("object")]
		public override string Object { get; set; }

        [JsonProperty("object_id")]
        public override string? ObjectId
        {
            get
            {
                if (string.IsNullOrEmpty(Object) || string.IsNullOrEmpty(Id))
                    return null;
                return $"cfl_{Id}";
            }
        }

        [JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("folder_name")]
		public string FolderName { get; set; }

		[JsonProperty("file_name")]
		public string FileName { get; set; }

		[JsonProperty("file_path")]
		public string FilePath { get; set; }

		[JsonProperty("case_id")]
		public string CaseId { get; set; }

		[JsonProperty("case_number")]
		public string CaseNumber { get; set; }

		[JsonProperty("created_at")]
		public DateTimeOffset CreatedAt { get; set; }

		[JsonProperty("updated_at")]
		public DateTimeOffset UpdatedAt { get; set; }

	}
}
