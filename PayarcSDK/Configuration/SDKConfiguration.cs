﻿namespace PayarcSDK.Configuration {
    public class SdkConfiguration {
        public string BaseUrl { get; set; }
        public string Environment { get; set; } = "prod"; // Default to production
        public string ApiVersion { get; set; } = "v1";    // Default API version
        public string BearerToken { get; set; }           // Bearer Token
    }
}
