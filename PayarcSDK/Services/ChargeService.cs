using AnyOfTypes;
using Newtonsoft.Json;
using PayarcSDK.Entities;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using JsonException = System.Text.Json.JsonException;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace PayarcSDK.Services
{
    public class ChargeService : CommonServices
    {
        private readonly HttpClient _httpClient;

		public ChargeService(HttpClient httpClient) : base(httpClient) {
			_httpClient = httpClient;
		}

		public async Task<BaseResponse?> Create(ChargeCreateOptions obj, ChargeCreateOptions? chargeData = null)

        {
            try
            {
                chargeData = chargeData ?? obj;
                var chargePayload = new ChargeRequestPayload();
                JsonConvert.PopulateObject(JsonConvert.SerializeObject(chargeData), chargePayload);
                if (chargeData.Source != null)
                {
                    var source = chargeData.Source;
                    if (source.Value.IsSecond)
                    {
                        var second = chargeData.Source.Value.Second;
                        JsonConvert.PopulateObject(JsonConvert.SerializeObject(second), chargePayload);
                    }
                }

                if (chargeData.ObjectId != null)
                {
                    var objectId = chargeData.ObjectId;
                    chargePayload.ObjectId = objectId.StartsWith("cus_") ? objectId.Substring("cus_".Length) : objectId;
                }

                if (chargeData.Source != null)
                {
                    var source = chargeData.Source.Value;
                    var isStr = chargeData.Source.Value.IsFirst;
                    switch (true)
                    {
                        case true when isStr && source.First.StartsWith("tok_"):
                            chargePayload.TokenId = source.First.Substring(4);
                            break;
                        case true when isStr && source.First.StartsWith("cus_"):
                            chargePayload.CustomerId = source.First.Substring(4);
                            break;
                        case true when isStr && source.First.StartsWith("card_"):
                            chargePayload.CardId = source.First.Substring(5);
                            break;
                        case true when (isStr && source.First.StartsWith("bnk_") || chargeData.SecCode != null):
                            chargePayload.BankAccountId = (isStr && source.First.StartsWith("bnk_"))
                                ? source.First.Substring(4)
                                : source.Second.BankAccountId?.Substring(4);
                            chargePayload.Type = "debit";
                            return await HandleChargeAsync(HttpMethod.Post, "achcharges",  chargePayload, "ACHCharge");
                        case true when (isStr && Regex.IsMatch(source.First, @"^\d")):
                            chargePayload.CardNumber = source;
                            break;
                    }
                }

                var idPrefixes = new Dictionary<string, int>
                {
                    { "TokenId", 3 },
                    { "CustomerId", 3 },
                    { "CardId", 4 }
                };
                NormalizeIDs(chargePayload, idPrefixes);
                return await HandleChargeAsync(HttpMethod.Post, "charges", chargePayload);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }


        public async Task<BaseResponse?> Retrieve(string chargeId)
        {
            try
            {
                var (endpoint, id) = DetermineEndpointAndId(chargeId);
                return await GetChargeDataAsync(HttpMethod.Get, endpoint, id, GetChargeType(chargeId));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        public async Task<ListBaseResponse?> List(BaseListOptions? options = null)
        {
            try
            {
                var parameters = new Dictionary<string, object?>
                    {
                        { "limit", options?.Limit ?? 25 },
                        { "page", options?.Page ?? 1 },
                        { "search", options?.Search }
                    }.Where(kvp => kvp.Value != null)
                    .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

                var query = BuildQueryString(parameters);
                return await GetChargesAsync("charges", query);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
		}

		public async Task<ListBaseResponse?> ListByAgentPayfac(BaseListOptions? options = null) {
			try {
				var parameters = new Dictionary<string, object?>
					{
						{ "limit", options?.Limit ?? 25 },
						{ "page", options?.Page ?? 1 },
						{ "search", options?.Search }
					}.Where(kvp => kvp.Value != null)
					.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

				var query = BuildQueryString(parameters);
				return await ListChargesByAgentPayfac("agent-hub/merchant-bridge/charges", query);
			} catch (Exception ex) {
				Console.WriteLine(ex);
				throw;
			}
		}

		public async Task<ListBaseResponse?> ListsByAgentTraditional(BaseListOptions? options = null) {
			try {
				var parameters = new Dictionary<string, object?>
					{
						{ "limit", options?.Limit ?? 25 },
						{ "page", options?.Page ?? 1 },
						{ "from_date", options?.From_Date },
						{ "to_date", options?.To_Date }
					}.Where(kvp => kvp.Value != null)
					.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

				var query = BuildQueryString(parameters);
				return await ListChargesByAgentTraditional("agent/charges", query);
			} catch (Exception ex) {
				Console.WriteLine(ex);
				throw;
			}
		}

		private async Task<ListBaseResponse?> GetChargesAsync(string endpoint, string? queryParams, string type = "Charge")
        {
            try
            {
                var url = $"{endpoint}?{queryParams}";
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                var response = await _httpClient.SendAsync(request);
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

                var responseData = JsonSerializer.Deserialize<Dictionary<string, object>>(responseBody);
                if (responseData == null || !responseData.TryGetValue("data", out var dataValue) ||
                    !(dataValue is JsonElement dataElement))
                {
                    throw new InvalidOperationException("Response data is invalid or missing.");
                }

                var rawData = dataElement.GetRawText();
                var jsonCharges = dataElement.Deserialize<List<Dictionary<string, object>>>();
                List<BaseResponse?>? charges = new List<BaseResponse?>();
                if (jsonCharges != null)
                {
                    for (int i = 0; i < jsonCharges.Count; i++)
                    {
                        var ch = TransformJsonRawObject(jsonCharges[i], JsonSerializer.Serialize(jsonCharges[i]), type);
                        charges?.Add(ch);
                    }
                }

                var pagination = new Dictionary<string, object>();
                if (responseData.TryGetValue("meta", out var metaValue) && metaValue is JsonElement metaElement)
                {
                    var paginationElement = metaElement.GetProperty("pagination");
                    pagination["total"] = paginationElement.GetProperty("total").GetInt32();
                    pagination["count"] = paginationElement.GetProperty("count").GetInt32();
                    pagination["per_page"] = paginationElement.GetProperty("per_page").GetInt32();
                    pagination["current_page"] = paginationElement.GetProperty("current_page").GetInt32();
                    pagination["total_pages"] = paginationElement.GetProperty("total_pages").GetInt32();
                }

                pagination?.Remove("links");

                return new BatchListResponse
                {
                    Data = charges,
                    Pagination = pagination,
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

		private async Task<ListBaseResponse?> ListChargesByAgentPayfac(string endpoint, string? queryParams, string type = "Charge") {
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
				var jsonCharges = dataElement.Deserialize<List<Dictionary<string, object>>>();
				List<BaseResponse?>? charges = new List<BaseResponse?>();
				if (jsonCharges != null) {
					for (int i = 0; i < jsonCharges.Count; i++) {
						var ch = TransformJsonRawObject(jsonCharges[i], JsonSerializer.Serialize(jsonCharges[i]), type);
						charges?.Add(ch);
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
					Data = charges,
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

		private async Task<ListBaseResponse?> ListChargesByAgentTraditional(string endpoint, string? queryParams, string type = "Charge") {
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
				var jsonCharges = dataElement.Deserialize<List<Dictionary<string, object>>>();
				List<BaseResponse?>? charges = new List<BaseResponse?>();
				if (jsonCharges != null) {
					for (int i = 0; i < jsonCharges.Count; i++) {
						var ch = TransformJsonRawObject(jsonCharges[i], JsonSerializer.Serialize(jsonCharges[i]), type);
						charges?.Add(ch);
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
					Data = charges,
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

		private Dictionary<string, string> GetParams(string endpoint)
        {
            if (endpoint == "charges")
            {
                return new Dictionary<string, string>
                {
                    { "include", "transaction_metadata,extra_metadata" }
                };
            }

            if (endpoint == "achcharges")
            {
                return new Dictionary<string, string>
                {
                    { "include", "review" }
                };
            }

            return new Dictionary<string, string>();
        }

        private (string endpoint, string id) DetermineEndpointAndId(string chargeId)
        {
            if (GetChargeType(chargeId) == "Charge")
            {
                return ("charges", chargeId.Substring(3));
            }

            if (GetChargeType(chargeId) == "ACHCharge")
            {
                return ("achcharges", chargeId.Substring(4));
            }

            throw new Exception("Invalid charge ID format.");
        }

        private string GetChargeType(string chargeId)
        {
            if(chargeId.StartsWith("ch_"))
                return "Charge";
            if(chargeId.StartsWith("ach_"))
                return "ACHCharge";
            throw new Exception("Invalid charge ID format.");
        }
        public void NormalizeIDs(ChargeRequestPayload payload, Dictionary<string, int> idPrefixes)
        {
            foreach (var (key, prefixLength) in idPrefixes)
            {
                var property = payload.GetType().GetProperty(key);
                if (property != null && property.PropertyType == typeof(string))
                {
                    var value = property.GetValue(payload) as string;
                    if (!string.IsNullOrEmpty(value) &&
                        value.StartsWith(key.ToLower().Substring(0, prefixLength) + "_"))
                    {
                        property.SetValue(payload, value.Substring(prefixLength + 1));
                    }
                }
            }
        }

        private async Task<BaseResponse?> GetChargeDataAsync(HttpMethod method, string endpoint, string chargeId, string type = "Charge")
        {
            try
            {
                var path = $"{endpoint}/{chargeId}";
                var parameters = GetParams(endpoint);
                var queryString = string.Join("&", parameters.Select(p => $"{Uri.EscapeDataString(p.Key)}={Uri.EscapeDataString(p.Value)}"));
                var fullPath = $"{path}?{queryString}";
                var request = new HttpRequestMessage(method, fullPath)
                {
                    Content = new StringContent(JsonSerializer.Serialize(parameters), Encoding.UTF8, "application/json")
                };
                var response = await _httpClient.SendAsync(request);
                var responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Response status code: {response.StatusCode}");
                // Console.WriteLine($"Response body: {responseBody}");
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

                var data = JsonSerializer.Deserialize<Dictionary<string, object>>(responseBody);
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

        private Dictionary<string, object>? GetAchChargeParams(AnyOf<string?, AchChargeResponseData?> charge,
            Dictionary<string, object>? parameters = null)
        {
            AchChargeResponseData? chargeObj = null;
            if (charge.IsFirst)
            {
                if (charge.First != null)
                {
                    chargeObj = (AchChargeResponseData?)GetChargeDataAsync(HttpMethod.Get, "charges", charge.First, "ACHCharge").Result;
                }
            }
            else
            {
                chargeObj = charge.Second;
            }

            if (chargeObj != null)
            {
                parameters ??= new Dictionary<string, object>();

                // Set default values for ACH-specific parameters
                parameters["type"] = "credit";
                parameters["amount"] = parameters.ContainsKey("amount") ? parameters["amount"] : chargeObj.Amount;
                parameters["sec_code"] = parameters.ContainsKey("sec_code") ? parameters["sec_code"] : chargeObj.SecCode;

                // Handle bank account-related parameters
                if (chargeObj.BankAccount?.Data?.ObjectId != null)
                {
                    parameters["bank_account_id"] = parameters.ContainsKey("bank_account_id")
                        ? parameters["bank_account_id"]
                        : chargeObj.BankAccount.Data.ObjectId;
                }

                if (parameters.TryGetValue("bank_account_id", out var bankAccountId) &&
                    bankAccountId is string bankAccountIdStr)
                {
                    if (bankAccountIdStr.StartsWith("bnk_"))
                    {
                        parameters["bank_account_id"] = bankAccountIdStr.Substring(4);
                    }
                }
            }


            return parameters;
        }

        public async Task<BaseResponse?> HandleChargeAsync(HttpMethod method, string path,
            ChargeRequestPayload chargeData, string type = "Charge")
        {
            try
            {
                HttpContent content;
                if (method == HttpMethod.Post && chargeData.Parameters != null)
                {
                    content = new StringContent(JsonConvert.SerializeObject(chargeData.Parameters), Encoding.UTF8, "application/json");
                    Console.WriteLine(JsonConvert.SerializeObject(chargeData.Parameters));
                }
                else
                {
                    content = new StringContent(chargeData.ToJson(), Encoding.UTF8, "application/json");
                    Console.WriteLine(chargeData.ToJson());
                }
                var request = new HttpRequestMessage(method, path)
                {
                    Content = content
                };
               

                var response = await _httpClient.SendAsync(request);
                var responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Response status code: {response.StatusCode}");
                // Console.WriteLine($"Response body: {responseBody}");

                if (!response.IsSuccessStatusCode)
                {
                    try
                    {
                        var errorData = JsonSerializer.Deserialize<Dictionary<string, object>>(responseBody);
                        Console.WriteLine($"Error details: {JsonSerializer.Serialize(errorData)}");
                        throw new InvalidOperationException($"HTTP error {response.StatusCode}: {responseBody}");
                    }
                    catch (JsonException jsonEx)
                    {
                        Console.WriteLine($"Failed to parse error JSON: {jsonEx.Message}");
                        throw new InvalidOperationException(
                            $"HTTP error {response.StatusCode}: Unable to parse error response.");
                    }
                }

                if (string.IsNullOrWhiteSpace(responseBody))
                {
                    throw new InvalidOperationException("Response body is empty.");
                }

                var data = JsonSerializer.Deserialize<Dictionary<string, object>>(responseBody);
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
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General error handling charge: {ex.Message}");
                throw;
            }
        }

        public async Task<BaseResponse?> CreateRefund(AnyOf<string?, BaseResponse> charge,
            Dictionary<string, object>? chargeData = null)
        {
            bool achRegular = false;
            string url = "charges";
            string msg = string.Empty;
            string? chargeId = string.Empty;
            Dictionary<string, object>? parameters = null;

            chargeId = charge.IsSecond ? charge.Second.ObjectId : (charge.IsFirst ? charge.First : null);

            try
            {
                var type = "Charge";
                if (chargeId != null)
                {
                    type = GetChargeType(chargeId);
                    if (chargeId.StartsWith("ch_"))
                    {
                        chargeId = chargeId.Substring(3);
                        url = $"{url}/{chargeId}/refunds";
                    }
                    else if (chargeId.StartsWith("ach_"))
                    {
                        achRegular = true;
                        parameters = GetAchChargeParams((AchChargeResponseData)charge, chargeData);
                        url = "achcharges";
                        msg = "ACH";
                    }
                }

                var response = await HandleChargeAsync(HttpMethod.Post, url, new ChargeRequestPayload
                {
                    Parameters = parameters
                },type);

                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing refund for {msg} charge: {ex.Message}");
                throw new InvalidOperationException($"Failed to process refund for {msg} charge", ex);
            }
        }

        public async Task<BaseResponse?> TipAdjust(AnyOf<string?, BaseResponse> charge, Dictionary<string, object>? tipData = null)
        {
            string url = "charges";
            string? chargeId = string.Empty;
            chargeId = charge.IsSecond ? charge.Second.ObjectId : (charge.IsFirst ? charge.First : null);
            try
            {
                var type = "Charge";
                if (chargeId != null)
                {
                    type = GetChargeType(chargeId);
                    if (chargeId.StartsWith("ch_"))
                    {
                        chargeId = chargeId.Substring(3);
                        url = $"{url}/{chargeId}/tip_adjustment";
                    }
                    else if (chargeId.StartsWith("ach_"))
                    {
                        throw new Exception("Tip adjustment is not applicable for ACH charges.");
                    }
                }
                var response = await HandleChargeAsync(HttpMethod.Post, url, new ChargeRequestPayload
                {
                    Parameters = tipData
                }, type);
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing tip adjustment for charge: {ex.Message}");
                throw new InvalidOperationException($"Failed to process tip adjustment for charge", ex);
            }
        }
    }
}