using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PayarcSDK.Entities
{
    public class DocumentParameters {
		[JsonPropertyName("DocumentDataBase64")]
		public string DocumentDataBase64 { get; set; }

		[JsonPropertyName("mimeType")]
		public string MimeType { get; set; }

		[JsonPropertyName("text")]
		public string Text { get; set; }

		[JsonPropertyName("message")]
		public string Message { get; set; }
	}
}
