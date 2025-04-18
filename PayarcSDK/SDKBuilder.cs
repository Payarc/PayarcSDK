using PayarcSDK.Configuration;

namespace PayarcSDK {
    public class SdkBuilder {
        private readonly SdkConfiguration _config = new();
        private HttpClient? _httpClient;

        public SdkBuilder Configure(Action<SdkConfiguration> configure) {
            configure(_config);

            // Default the environment to "prod" if not specified
            _config.Environment ??= "prod";

            // Resolve BaseUrl based on the Environment
            _config.BaseUrl = _config.Environment switch {
                "prod" => "https://api.payarc.net",
                "sandbox" => "https://testapi.payarc.net",
                "payarcConnect" => "https://payarcconnectapi.curvpos.com/",
                "payarcConnectDev" => "'https://payarcconnectapi.curvpos.dev/",
                _ => _config.BaseUrl // Use the provided custom URL if not prod or sandbox
            };

            // Validate that BaseUrl is configured
            if (string.IsNullOrWhiteSpace(_config.BaseUrl)) {
                throw new InvalidOperationException("BaseUrl must be configured. Please specify a valid environment or provide a custom BaseUrl.");
            }

            // Append API version to the BaseUrl            
            _config.BaseUrl = _config.Environment is not ("payarcConnect" or "payarcConnectDev") ? $"{_config.BaseUrl}/{_config.ApiVersion}/" : _config.BaseUrl;

            return this;
        }

        public SdkBuilder UseHttpClient(HttpClient httpClient) {
            _httpClient = httpClient;
            return this;
        }

        public Payarc Build() {
            // Ensure the BaseUrl is resolved
            if (string.IsNullOrEmpty(_config.BaseUrl)) {
                throw new InvalidOperationException("BaseUrl must be configured.");
            }

            // Ensure BearerToken is provided
            if (string.IsNullOrWhiteSpace(_config.BearerToken)) {
                throw new InvalidOperationException("BearerToken is missing. Please provide a valid BearerToken.");
            }

            // Use the provided HttpClient or create a new one
            var httpClient = _httpClient ?? new HttpClient { BaseAddress = new Uri(_config.BaseUrl) };

            // Add the Authorization header with Bearer token
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _config.BearerToken);

			var userAgent = $"sdk-csharp/{_config.ApiVersion}";
			if (!httpClient.DefaultRequestHeaders.UserAgent.TryParseAdd(userAgent)) {
				throw new InvalidOperationException("Invalid User-Agent header value.");
			}

			// Return the Payarc instance (facade for all services)
			return new Payarc(httpClient, _config);
        }
    }
}
