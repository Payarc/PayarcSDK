using Newtonsoft.Json;
using PayarcSDK.Entities;

namespace PayarcSDK.Entities.InstructionalFunding {
	public class InstructionalFundingResponseData : BaseResponse {
        [JsonProperty("object")]
        public string? Object { get; set; }

        [JsonProperty("object_id")]
        public override string? ObjectId
        {
            get
            {
                if (string.IsNullOrEmpty(Object) || string.IsNullOrEmpty(Id))
                    return null;
                return $"cspl_{Id}";
            }
        }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("amount")]
        public int amount { get; set; }

        [JsonProperty("amount_formatted")]
        public string amount_formatted { get; set; }

        [JsonProperty("percent")]
        public string percent { get; set; }

        [JsonProperty("mid")]
        public string mid { get; set; }

        [JsonProperty("status")]
        public string status { get; set; }

        [JsonProperty("description")]
        public object description { get; set; }

        [JsonProperty("created_at")]
        public DateTime created_at { get; set; }

        [JsonProperty("updated_at")]
        public DateTime updated_at { get; set; }

        [JsonProperty("charge")]
        public ChargeWrapper? ChargeWrapper { get; set; }
    }
}

public class ChargeWrapper
{
    [JsonProperty("data")]
    public ChargeResponseData? Charges { get; set; }
}