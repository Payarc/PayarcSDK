using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayarcSDK.Entities.Payee
{
    public class PayeeListResponse : ListBaseResponse {
        [JsonProperty("payees")]
        public override List<BaseResponse?>? Data { get; set; }
    }
}
