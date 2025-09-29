using Newtonsoft.Json.Linq;
using PayarcSDK.Entities;
using PayarcSDK.Entities.Payee;
using PayarcSDK.Entities.SplitCampaign;
using System.Text;
using System.Text.Json;

namespace PayarcSDK.Services {
    public class PayeeService : CommonServices {
        private readonly HttpClient _httpClient;

        public PayeeService(HttpClient httpClient) : base(httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<BaseResponse> Create(PayeeRequestData payeeData)
        {
            return await CreatePayeeAsync(payeeData);
        }

        public async Task<ListBaseResponse> List(BaseListOptions? options = null)
        {
            return await GetAllPayeeAsync(options);
        }

        public async Task<bool> Delete(string customerId) {
            return await DeletePayee(customerId);
        }

        private async Task<BaseResponse> CreatePayeeAsync(PayeeRequestData payeeData)
        {
            return await CreatePayee("agent-hub/apply/payees", payeeData);
        }
        private async Task<BaseResponse?> CreatePayee(string url, PayeeRequestData payeeData, string type = "Payee")
        {
            try
            {
                var content = new StringContent(payeeData.ToJson(), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(url, content);
                response.EnsureSuccessStatusCode();
                var responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Response status code: {response.StatusCode}");
                if (!response.IsSuccessStatusCode)
                {
                    var errorData = JsonSerializer.Deserialize<Dictionary<string, object>>(responseContent);
                    Console.WriteLine($"Error details: {JsonSerializer.Serialize(errorData)}");
                    throw new InvalidOperationException($"HTTP error {response.StatusCode}: {responseContent}");
                }

                if (string.IsNullOrWhiteSpace(responseContent))
                {
                    throw new InvalidOperationException("Response body is empty.");
                }

                var data = JsonSerializer.Deserialize<Dictionary<string, object>>(responseContent);
                if (data != null && data.TryGetValue("data", out var dataValue) && dataValue is JsonElement dataElement)
                {
                    var dataDict = JsonSerializer.Deserialize<Dictionary<string, object>>(dataElement.GetRawText());
                    if (dataDict != null)
                    {
                        return TransformJsonRawObject(dataDict, dataElement.GetRawText(), type);
                    }
                }
                throw new InvalidOperationException("Response data is invalid or missing.");
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP error processing charge: {ex.Message}");
                throw;
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"JSON error processing charge: {ex.Message}");
                throw new InvalidOperationException("Failed to process JSON response.", ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General error handling charge: {ex.Message}");
                throw;
            }
        }
        private async Task<ListBaseResponse> GetAllPayeeAsync(BaseListOptions? options = null)
        {
            try
            {
                var parameters = new Dictionary<string, object>
                {
                    { "include", options?.Include ?? "appData" },
                    { "limit", options?.Limit ?? 25 },
                    { "page", options?.Page ?? 1 }
                };
                var query = BuildQueryString(parameters);
                return await GetPayeeAsync("agent-hub/apply/payees", query);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        private async Task<ListBaseResponse> GetPayeeAsync(string url, string? queryParams, string type = "PayeeList")
        {
            try
            {
                if (queryParams != null)
                {
                    url = $"{url}?{queryParams}";
                }

                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                var responseBody = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"Response status code: {response.StatusCode}");
                if (!response.IsSuccessStatusCode)
                {
                    var errorData = JsonSerializer.Deserialize<Dictionary<string, object>>(responseBody);
                    Console.WriteLine($"Error details: {JsonSerializer.Serialize(errorData)}");
                    throw new InvalidOperationException($"HTTP error {response.StatusCode}: {responseBody}");
                }

                if (string.IsNullOrWhiteSpace(responseBody))
                {
                    throw new InvalidOperationException("Response body is empty.");
                }
                var wrappedJson = $"{{\"data\":{responseBody}}}";
                var responseData = JsonSerializer.Deserialize<Dictionary<string, object>>(wrappedJson);
                if (responseData == null || !responseData.TryGetValue("data", out var dataValue) ||
                    !(dataValue is JsonElement dataElement))
                {
                    throw new InvalidOperationException("Response data is invalid or missing.");
                }

                var rawData = dataElement.GetRawText();
                var jsonPayees = dataElement.Deserialize<List<Dictionary<string, object>>>();
                List<BaseResponse?>? payees = new List<BaseResponse?>();
                if (jsonPayees != null)
                {
                    for (int i = 0; i < jsonPayees.Count; i++)
                    {
                        var payee = TransformJsonRawObject(jsonPayees[i], JsonSerializer.Serialize(jsonPayees[i]), type);
                        payees?.Add(payee);
                    }
                }

                return new PayeeListResponse
                {
                    Data = payees,
                    RawData = rawData
                };
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP error processing charge: {ex.Message}");
                throw;
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"JSON error processing charge: {ex.Message}");
                throw new InvalidOperationException("Failed to process JSON response.", ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General error handling charge: {ex.Message}");
                throw;
            }
        }

        private async Task<bool> DeletePayee(string payeeId) {
            payeeId = payeeId.StartsWith("appy_") ? payeeId.Substring(5) : payeeId;
            await DeletePayeeAsync($"agent-hub/apply/payees/{payeeId}");
            return true;
        }

        private async Task DeletePayeeAsync(string url) {
            var response = await _httpClient.DeleteAsync(url);
            response.EnsureSuccessStatusCode();
        }
    }
}
