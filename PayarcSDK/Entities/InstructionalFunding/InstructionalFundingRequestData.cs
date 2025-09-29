using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayarcSDK.Entities.InstructionalFunding
{
    public class InstructionalFundingRequestData
    {
        [JsonProperty("mid")]
        public string Mid { get; set; }

        [JsonProperty("amount")]
        public int Amount { get; set; }

        [JsonIgnore]
        [JsonProperty("include")]
        public string? Include { get; set; }

        public string ToJson()
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.Indented
            };

            return JsonConvert.SerializeObject(this, settings);
        }
    }
}
