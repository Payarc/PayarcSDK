using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayarcSDK.Entities.ApplicationService
{
	public class MerchantDocument {
		[JsonProperty("DocumentType")]
		public string DocumentType { get; set; }

		[JsonProperty("DocumentName")]
		public string DocumentName { get; set; }

		[JsonIgnore]
		[JsonProperty("DocumentIndex")]
		public int? DocumentIndex { get; set; }

		[JsonProperty("DocumentCode")]
		public string? DocumentCode { get; set; }

		[JsonProperty("DocumentDataBase64")]
		public string DocumentDataBase64 { get; set; }
	}
}
