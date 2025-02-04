using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayarcSDK.Entities.Dispute
{
    public class DisputeListResponse : ListBaseResponse {
		[JsonProperty("cases")]
		public override List<BaseResponse?>? Data { get; set; }
	}
}
