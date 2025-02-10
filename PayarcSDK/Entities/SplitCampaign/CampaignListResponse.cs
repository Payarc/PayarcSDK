using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayarcSDK.Entities.SplitCampaign
{
    public class CampaignListResponse : ListBaseResponse {
		[JsonProperty("campaigns")]
		public override List<BaseResponse?>? Data { get; set; }
	}
}
