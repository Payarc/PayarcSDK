namespace PayarcSDK.Entities.PayarcConnectService {
    public class ServerInfoResponse {
        public string? ServerName { get; set; }
        public DateTime? ServerTime { get; set; }
        public string? AppEnvironment { get; set; }
        public string? AppVersion { get; set; }
        public string? Url { get; set; }
    }
}