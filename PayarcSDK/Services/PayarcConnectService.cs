using AnyOfTypes;
using PayarcSDK.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PayarcSDK.Services {
    public class PayarcConnectService {
        private readonly HttpClient _httpClient;
        private readonly SdkConfiguration _config;

        private string PayarcConnectAccessToken { get; set; }

        public PayarcConnectService(HttpClient httpClient, SdkConfiguration config) {
            _httpClient = httpClient;
            _config = config;
        }

        public async Task<object> Login() {
            var seed = new { source = "Payarc Connect Login" };

            try {
                var requestBody = new {
                    SecretKey = this._config.BearerToken,
                };

                var url = $"{this._config.BaseUrl}/Login";
                var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(url, content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode) {
                    var responseData = JsonSerializer.Deserialize<LoginResponse>(responseContent);
                    var accessToken = responseData?.BearerTokenInfo?.AccessToken;

                    if (!string.IsNullOrEmpty(accessToken)) {
                        this.PayarcConnectAccessToken = accessToken;
                    } else {
                        return "";
                        //return this.PayarcConnectError(seed, responseData);
                    }

                    return responseData;
                } else {
                    return "";
                    //return this.ManageError(seed, responseContent);
                }
            } catch (Exception ex) {
                return "";
                //return this.ManageError(seed, new { Error = ex.Message });
            }
        }

        public class LoginResponse {
            public BearerTokenInfo BearerTokenInfo { get; set; }
        }

        public class BearerTokenInfo {
            public string AccessToken { get; set; }
        }
    }
}
