using PayarcSDK.Entities;
using PayarcSDK.Entities.InstructionalFunding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PayarcSDK.Services {
	public class InstructionalFundingService : CommonServices {
        private readonly HttpClient _httpClient;

        public InstructionalFundingService(HttpClient httpClient) : base(httpClient) {
            _httpClient = httpClient;
        }

        public async Task<BaseResponse> Create(InstructionalFundingRequestData instructionalFundingData)
        {
            return await CreateInstructionalFundingAsync(instructionalFundingData);
        }

        public async Task<ListBaseResponse?> List(BaseListOptions? options = null) {
            try {
                var parameters = new Dictionary<string, object?>
                    {
                        { "limit", options?.Limit ?? 25 },
                        { "page", options?.Page ?? 1 },
                        { "include", options?.Include ?? "charge" }
                    }.Where(kvp => kvp.Value != null)
                    .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

                var query = BuildQueryString(parameters);
                return await ListInstructionalFunding("instructional_funding", query);
            } catch (Exception ex) {
                Console.WriteLine(ex);
                throw;
            }
        }
        private async Task<BaseResponse> CreateInstructionalFundingAsync(InstructionalFundingRequestData instructionalFundingData)
        {
            return await CreateInstructionalFunding("instructional_funding", instructionalFundingData);
        }

        private async Task<ListBaseResponse?> ListInstructionalFunding(string endpoint, string? queryParams, string type = "ChargeSplit") {
            try {
                var url = $"{endpoint}?{queryParams}";
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                var response = await _httpClient.SendAsync(request);
                var responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Response status code: {response.StatusCode}");
                if (!response.IsSuccessStatusCode) {
                    var errorData = JsonSerializer.Deserialize<Dictionary<string, object>>(responseBody);
                    Console.WriteLine($"Error details: {JsonSerializer.Serialize(errorData)}");
                    throw new InvalidOperationException($"HTTP error {response.StatusCode}: {responseBody}");
                }

                if (string.IsNullOrWhiteSpace(responseBody)) {
                    throw new InvalidOperationException("Response body is empty.");
                }

                var responseData = JsonSerializer.Deserialize<Dictionary<string, object>>(responseBody);
                if (responseData == null || !responseData.TryGetValue("data", out var dataValue) ||
                    !(dataValue is JsonElement dataElement)) {
                    throw new InvalidOperationException("Response data is invalid or missing.");
                }

                var rawData = dataElement.GetRawText();
                var jsonChargeSplits = dataElement.Deserialize<List<Dictionary<string, object>>>();
                List<BaseResponse?>? chargeResponses = new List<BaseResponse?>();
                if (jsonChargeSplits != null) {
                    for (int i = 0; i < jsonChargeSplits.Count; i++) {
                        var ch = TransformJsonRawObject(jsonChargeSplits[i], JsonSerializer.Serialize(jsonChargeSplits[i]), type);
                        chargeResponses?.Add(ch);
                    }
                }

                var pagination = new Dictionary<string, object>();
                if (responseData.TryGetValue("meta", out var metaValue) && metaValue is JsonElement metaElement) {
                    var paginationElement = metaElement.GetProperty("pagination");
                    pagination["total"] = paginationElement.GetProperty("total").GetInt32();
                    pagination["count"] = paginationElement.GetProperty("count").GetInt32();
                    pagination["per_page"] = paginationElement.GetProperty("per_page").GetInt32();
                    pagination["current_page"] = paginationElement.GetProperty("current_page").GetInt32();
                    pagination["total_pages"] = paginationElement.GetProperty("total_pages").GetInt32();
                }

                pagination?.Remove("links");

                return new BatchListResponse {
                    Data = chargeResponses,
                    Pagination = pagination,
                    RawData = rawData
                };
            } catch (HttpRequestException ex) {
                Console.WriteLine($"HTTP error processing charge: {ex.Message}");
                throw;
            } catch (JsonException ex) {
                Console.WriteLine($"JSON error processing charge: {ex.Message}");
                throw new InvalidOperationException("Failed to process JSON response.", ex);
            } catch (Exception ex) {
                Console.WriteLine($"General error handling charge: {ex.Message}");
                throw;
            }
        }
        private async Task<BaseResponse?> CreateInstructionalFunding(string endpoint, InstructionalFundingRequestData instructionalFundingData, string type = "ChargeSplit")
        {
            try
            {
                var parameters = new Dictionary<string, object?>
                    {
                        { "include", instructionalFundingData.Include ?? "charge" }
                    }.Where(kvp => kvp.Value != null)
                    .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
                var query = BuildQueryString(parameters);
                var url = $"{endpoint}?{query}";
                var content = new StringContent(instructionalFundingData.ToJson(), Encoding.UTF8, "application/json");
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
    }
}
