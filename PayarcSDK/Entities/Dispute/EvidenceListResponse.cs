using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayarcSDK.Entities {
	public class EvidenceListResponse : ListBaseResponse {
		[JsonProperty("Evidence")]
		public override List<BaseResponse?>? Data { get; set; }
	}
}
