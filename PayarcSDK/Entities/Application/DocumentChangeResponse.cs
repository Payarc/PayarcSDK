using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayarcSDK.Entities
{
    public class DocumentChangeResponse : BaseResponse {
		[JsonProperty("object")]
		public override string? Object { get; set; }

		[JsonProperty("object_id")]
		public override string? ObjectId { get; set; }

		[JsonIgnore]
		[JsonProperty("MerchantCode")]
		public string? MerchantCode { get; set; }

        [JsonProperty("MerchantDocuments")]
        public List<MerchantDocuments>? MerchantDocuments { get; set; }
    }

    public class MerchantDocuments : BaseResponse {
        [JsonProperty("object")]
        public override string Object { get; set; }

        [JsonProperty("object_id")]
        public override string? ObjectId { get; set; }

        [JsonProperty("DocumentCode")]
        public string DocumentCode { get; set; }
    }
}
