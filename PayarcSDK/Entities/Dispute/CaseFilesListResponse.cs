using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayarcSDK.Entities
{
	public class CaseFileListResponse : ListBaseResponse {
		[JsonProperty("CaseFile")]
		public override List<BaseResponse?>? Data { get; set; }
	}
}
