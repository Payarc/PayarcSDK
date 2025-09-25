using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayarcSDK.Entities
{
    public class SubAgentResponseData : BaseResponse {
        [JsonProperty("object")]
        public string Object { get; set; }

        [JsonProperty("object_id")]
        public override string? ObjectId
        {
            get
            {
                if (string.IsNullOrEmpty(Object) || string.IsNullOrEmpty(Id))
                    return null;
                return $"usr_{Id}";
            }
        }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("is_agent")]
        public long IsAgent { get; set; }

        [JsonProperty("is_sub_agent")]
        public long IsSubAgent { get; set; }

        [JsonProperty("agent_id")]
        public long AgentId { get; set; }
    }
}
