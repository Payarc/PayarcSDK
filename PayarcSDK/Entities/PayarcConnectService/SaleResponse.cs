using Newtonsoft.Json.Linq;

namespace PayarcSDK.Entities.PayarcConnectService {
    public class SaleResponse {
        public JObject? PaxResponse { get; set; }
        public int ErrorCode { get; set; }
        public string? ErrorMessage { get; set; }
    }
}