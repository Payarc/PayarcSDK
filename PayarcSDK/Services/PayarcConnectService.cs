using PayarcSDK.Configuration;
using PayarcSDK.Entities.PayarcConnectService;
using PayarcSDK.Exceptions;
using System.Text;
using System.Text.Json;

namespace PayarcSDK.Services {
    public class PayarcConnectService {
        private readonly HttpClient _httpClient;
        private readonly SdkConfiguration _config;

        private string? PayarcConnectAccessToken { get; set; }

        public PayarcConnectService(HttpClient httpClient, SdkConfiguration config) {
            _httpClient = httpClient;
            _config = config;
        }

        public async Task<LoginResponse> Login() {
            var seed = new { source = "Payarc Connect Login" };

            try {
                var requestBody = new {
                    SecretKey = _config.BearerToken,
                };

                var url = $"{_config.BaseUrl}Login";
                var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(url, content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode) {
                    var responseData = JsonSerializer.Deserialize<LoginResponse>(responseContent);
                    var accessToken = responseData?.BearerTokenInfo?.AccessToken;

                    if (!string.IsNullOrEmpty(accessToken)) {
                        PayarcConnectAccessToken = accessToken;
                    } else {
                        throw new AccessTokenMissingException("Access token is missing from the response.");
                    }

                    return responseData ?? throw new InvalidResponseFormatException("The response content could not be deserialized.", new JsonException());
                } else {
                    throw new HttpRequestException($"HTTP request failed with status code: {response.StatusCode}. Response: {responseContent}");
                }
            } catch (HttpRequestException ex) {
                throw new HttpRequestException("Error occurred during the HTTP request.", ex);
            } catch (JsonException ex) {
                throw new InvalidResponseFormatException("The response content could not be deserialized.", ex);
            } catch (Exception ex) {
                throw new Exception("An unexpected error occurred in the Payarc Connect service.", ex);
            }
        }

        public async Task<SaleResponse> Sale(string tenderType, string ecrRefNum, string amount, string deviceSerialNo) {
            var seed = new { source = "Payarc Connect Sale" };

            try {
                var requestBody = new {
                    TenderType = tenderType,
                    TransType = "SALE",
                    ECRRefNum = ecrRefNum,
                    Amount = amount,
                    DeviceSerialNo = deviceSerialNo
                };

                var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

                var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"{_config.BaseUrl}Transactions") {
                    Content = content
                };
                requestMessage.Headers.Add("Authorization", $"Bearer {PayarcConnectAccessToken}");

                var response = await _httpClient.SendAsync(requestMessage);

                var responseContent = await response.Content.ReadAsStringAsync();

                var responseData = JsonSerializer.Deserialize<SaleResponse>(responseContent);

                if (responseData?.ErrorCode != 0) {
                    throw new Exception($"PayarcConnect returned an error during the sale transaction. ErrorCode: {responseData?.ErrorCode}, ErrorMessage: {responseData?.ErrorMessage}");
                }

                return responseData;
            } catch (HttpRequestException ex) {
                throw new HttpRequestException("Error occurred during the sale transaction HTTP request.", ex);
            } catch (JsonException ex) {
                throw new InvalidResponseFormatException("The response content from the sale transaction could not be deserialized.", ex);
            } catch (Exception ex) {
                throw new Exception("An unexpected error occurred in the Payarc Connect sale transaction.", ex);
            }
        }

        public async Task<SaleResponse> Void(string payarcTransactionId, string deviceSerialNo) {
            var seed = new { source = "Payarc Connect Void" };

            try {
                var requestBody = new {
                    TransType = "VOID",
                    PayarcTransactionId = payarcTransactionId,
                    DeviceSerialNo = deviceSerialNo
                };

                var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

                var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"{_config.BaseUrl}Transactions") {
                    Content = content
                };
                requestMessage.Headers.Add("Authorization", $"Bearer {PayarcConnectAccessToken}");

                var response = await _httpClient.SendAsync(requestMessage);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode) {
                    throw new Exception($"Void transaction failed with status code {response.StatusCode}. Response: {responseContent}");
                }

                var responseData = JsonSerializer.Deserialize<SaleResponse>(responseContent);

                if (responseData?.ErrorCode != 0) {
                    throw new Exception($"PayarcConnect returned an error during the void transaction. ErrorCode: {responseData?.ErrorCode}, ErrorMessage: {responseData?.ErrorMessage}");
                }

                return responseData;
            } catch (HttpRequestException ex) {
                throw new HttpRequestException("Error occurred during the void transaction HTTP request.", ex);
            } catch (JsonException ex) {
                throw new InvalidResponseFormatException("The response content from the void transaction could not be deserialized.", ex);
            } catch (Exception ex) {
                throw new Exception("An unexpected error occurred during the Payarc Connect void transaction.", ex);
            }
        }

        public async Task<SaleResponse> Refund(string amount, string payarcTransactionId, string deviceSerialNo) {
            var seed = new { source = "Payarc Connect Refund" };

            try {
                var requestBody = new {
                    TransType = "REFUND",
                    Amount = amount,
                    PayarcTransactionId = payarcTransactionId,
                    DeviceSerialNo = deviceSerialNo
                };

                var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

                var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"{_config.BaseUrl}Transactions") {
                    Content = content
                };
                requestMessage.Headers.Add("Authorization", $"Bearer {PayarcConnectAccessToken}");

                var response = await _httpClient.SendAsync(requestMessage);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode) {
                    throw new Exception($"Refund transaction failed with status code {response.StatusCode}. Response: {responseContent}");
                }

                var responseData = JsonSerializer.Deserialize<SaleResponse>(responseContent);

                if (responseData?.ErrorCode != 0) {
                    throw new Exception($"PayarcConnect returned an error during the refund transaction. ErrorCode: {responseData?.ErrorCode}, ErrorMessage: {responseData?.ErrorMessage}");
                }

                return responseData;
            } catch (HttpRequestException ex) {
                throw new HttpRequestException("Error occurred during the refund transaction HTTP request.", ex);
            } catch (JsonException ex) {
                throw new InvalidResponseFormatException("The response content from the refund transaction could not be deserialized.", ex);
            } catch (Exception ex) {
                throw new Exception("An unexpected error occurred during the Payarc Connect refund transaction.", ex);
            }
        }

        public async Task<SaleResponse> BlindCredit(string ecrRefNum, string amount, string token, string expDate, string deviceSerialNo) {
            try {
                var requestBody = new {
                    TransType = "RETURN",
                    ECRRefNum = ecrRefNum,
                    Amount = amount,
                    Token = token,
                    ExpDate = expDate,
                    DeviceSerialNo = deviceSerialNo
                };

                var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

                var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"{_config.BaseUrl}Transactions") {
                    Content = content
                };
                requestMessage.Headers.Add("Authorization", $"Bearer {PayarcConnectAccessToken}");

                var response = await _httpClient.SendAsync(requestMessage);
                var responseContent = await response.Content.ReadAsStringAsync();

                var responseData = JsonSerializer.Deserialize<SaleResponse>(responseContent);

                if (responseData?.ErrorCode != 0) {
                    throw new Exception($"Payarc Connect returned an error during the Blind Credit transaction. ErrorCode: {responseData?.ErrorCode}, ErrorMessage: {responseData?.ErrorMessage}");
                }

                return responseData;
            } catch (HttpRequestException ex) {
                throw new HttpRequestException("Error occurred during the Blind Credit transaction HTTP request.", ex);
            } catch (JsonException ex) {
                throw new InvalidResponseFormatException("The response content from the Blind Credit transaction could not be deserialized.", ex);
            } catch (Exception ex) {
                throw new Exception("An unexpected error occurred during the Payarc Connect Blind Credit transaction.", ex);
            }
        }

        public async Task<SaleResponse> Auth(string ecrRefNum, string amount, string deviceSerialNo) {
            try {
                var requestBody = new {
                    TransType = "AUTH",
                    ECRRefNum = ecrRefNum,
                    Amount = amount,
                    DeviceSerialNo = deviceSerialNo
                };

                var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"{_config.BaseUrl}Transactions") {
                    Content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json")
                };
                requestMessage.Headers.Add("Authorization", $"Bearer {PayarcConnectAccessToken}");

                var response = await _httpClient.SendAsync(requestMessage);
                var responseContent = await response.Content.ReadAsStringAsync();
                var responseData = JsonSerializer.Deserialize<SaleResponse>(responseContent);

                if (responseData?.ErrorCode != 0) {
                    throw new Exception($"Payarc Connect returned an error during the AUTH transaction. ErrorCode: {responseData?.ErrorCode}, ErrorMessage: {responseData?.ErrorMessage}");
                }

                return responseData;
            } catch (HttpRequestException ex) {
                throw new HttpRequestException("Error occurred during the AUTH transaction HTTP request.", ex);
            } catch (JsonException ex) {
                throw new InvalidResponseFormatException("The response content from the AUTH transaction could not be deserialized.", ex);
            } catch (Exception ex) {
                throw new Exception("An unexpected error occurred during the Payarc Connect AUTH transaction.", ex);
            }
        }

        public async Task<SaleResponse> PostAuth(string ecrRefNum, string origRefNum, string amount, string deviceSerialNo) {
            try {
                var requestBody = new {
                    TransType = "POSTAUTH",
                    ECRRefNum = ecrRefNum,
                    OrigRefNum = origRefNum,
                    Amount = amount,
                    DeviceSerialNo = deviceSerialNo
                };

                var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"{_config.BaseUrl}Transactions") {
                    Content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json")
                };
                requestMessage.Headers.Add("Authorization", $"Bearer {PayarcConnectAccessToken}");

                var response = await _httpClient.SendAsync(requestMessage);
                var responseContent = await response.Content.ReadAsStringAsync();
                var responseData = JsonSerializer.Deserialize<SaleResponse>(responseContent);

                if (responseData?.ErrorCode != 0) {
                    throw new Exception($"Payarc Connect returned an error during the POSTAUTH transaction. ErrorCode: {responseData?.ErrorCode}, ErrorMessage: {responseData?.ErrorMessage}");
                }

                return responseData;
            } catch (HttpRequestException ex) {
                throw new HttpRequestException("Error occurred during the POSTAUTH transaction HTTP request.", ex);
            } catch (JsonException ex) {
                throw new InvalidResponseFormatException("The response content from the POSTAUTH transaction could not be deserialized.", ex);
            } catch (Exception ex) {
                throw new Exception("An unexpected error occurred during the Payarc Connect POSTAUTH transaction.", ex);
            }
        }

        public async Task<SaleResponse> LastTransaction(string deviceSerialNo) {
            try {
                var requestUri = $"{_config.BaseUrl}LastTransaction?DeviceSerialNo={Uri.EscapeDataString(deviceSerialNo)}";

                var requestMessage = new HttpRequestMessage(HttpMethod.Get, requestUri);
                requestMessage.Headers.Add("Authorization", $"Bearer {PayarcConnectAccessToken}");

                var response = await _httpClient.SendAsync(requestMessage);
                var responseContent = await response.Content.ReadAsStringAsync();
                var responseData = JsonSerializer.Deserialize<SaleResponse>(responseContent);

                if (responseData?.ErrorCode != 0) {
                    throw new Exception($"Payarc Connect returned an error during the Last Transaction request. ErrorCode: {responseData?.ErrorCode}, ErrorMessage: {responseData?.ErrorMessage}");
                }

                return responseData;
            } catch (HttpRequestException ex) {
                throw new HttpRequestException("Error occurred during the Last Transaction HTTP request.", ex);
            } catch (JsonException ex) {
                throw new InvalidResponseFormatException("The response content from the Last Transaction request could not be deserialized.", ex);
            } catch (Exception ex) {
                throw new Exception("An unexpected error occurred during the Payarc Connect Last Transaction request.", ex);
            }
        }

        public async Task<ServerInfoResponse> ServerInfo() {
            try {
                var requestUri = $"{_config.BaseUrl}ServerInfo";

                var requestMessage = new HttpRequestMessage(HttpMethod.Get, requestUri);

                var response = await _httpClient.SendAsync(requestMessage);
                var responseContent = await response.Content.ReadAsStringAsync();
                var responseData = JsonSerializer.Deserialize<ServerInfoResponse>(responseContent);

                if (responseData == null) {
                    throw new NullReferenceException("The response content from the Server Info request could not be deserialized.");
                }

                return responseData;
            } catch (HttpRequestException ex) {
                throw new HttpRequestException("Error occurred during the Server Info HTTP request.", ex);
            } catch (JsonException ex) {
                throw new InvalidResponseFormatException("The response content from the Server Info request could not be deserialized.", ex);
            } catch (Exception ex) {
                throw new Exception("An unexpected error occurred during the Payarc Connect Server Info request.", ex);
            }
        }

        public async Task<TerminalResponse> Terminals() {
            try {
                var requestUri = $"{_config.BaseUrl}Terminals";

                var requestMessage = new HttpRequestMessage(HttpMethod.Get, requestUri);
                requestMessage.Headers.Add("Authorization", $"Bearer {PayarcConnectAccessToken}");

                var response = await _httpClient.SendAsync(requestMessage);
                var responseContent = await response.Content.ReadAsStringAsync();
                var responseData = JsonSerializer.Deserialize<TerminalResponse>(responseContent);

                if (responseData?.ErrorCode != 0) {
                    throw new Exception($"Payarc Connect returned an error during the Terminals request. ErrorCode: {responseData?.ErrorCode}, ErrorMessage: {responseData?.ErrorMessage}");
                }

                return responseData;
            } catch (HttpRequestException ex) {
                throw new Exception("Error occurred during the Terminals HTTP request.", ex);
            } catch (JsonException ex) {
                throw new InvalidResponseFormatException("The response content from the Terminals request could not be deserialized.", ex);
            } catch (Exception ex) {
                throw new Exception("An unexpected error occurred during the Payarc Connect Terminals request.", ex);
            }
        }
    }
}