using Newtonsoft.Json;

namespace PayarcSDK.Entities.Billing.Subscriptions;

public class SubscriptionListResponse : ListBaseResponse
{
    [JsonProperty("subscriptions")]
    public override List<BaseResponse?>? Data { get; set; }
}