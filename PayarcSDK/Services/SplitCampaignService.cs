using AnyOfTypes;
using Newtonsoft.Json.Linq;
using PayarcSDK.Entities;
using PayarcSDK.Entities.SplitCampaign;
using System.Text;
using System.Text.Json;

namespace PayarcSDK.Services {
	public class SplitCampaignService : CommonServices {
		private readonly HttpClient _httpClient;

		public SplitCampaignService(HttpClient httpClient) : base(httpClient) {
			_httpClient = httpClient;
		}

		public async Task<BaseResponse> Create(SplitCampaignRequestData campaignData) {
			return await CreateCampaignAsync(campaignData);
		}

		public async Task<ListBaseResponse> List(BaseListOptions? options = null) {
			return await GetAllCampaignsAsync(options);
		}

		public async Task<BaseResponse> Retrieve(AnyOf<string?, CampaignResponseData> campaign) {
			string? campaignId = string.Empty;
			campaignId = campaign.IsSecond ? campaign.Second.ObjectId : campaign.First;
			return await ReceiveCampaignDetailsAsync(campaignId);
		}

		public async Task<BaseResponse> Update(AnyOf<string?, CampaignResponseData> campaign, SplitCampaignRequestData updatedData = null) {
			string? campaignId = string.Empty;
			campaignId = campaign.IsSecond ? campaign.Second.ObjectId : campaign.First;
			return await UpdateCampaignAsync(campaignId, updatedData);
		}

		public async Task<ListBaseResponse> ListAccounts() {
			return await GetAllAccounts("account/my-accounts", null);
		}

		/// Create a new campaign.
		private async Task<BaseResponse> CreateCampaignAsync(SplitCampaignRequestData campaignData) {
			return await CreateSplitCampaign("agent-hub/campaigns", campaignData);
		}

		// Retrieve all campaigns.
		private async Task<ListBaseResponse> GetAllCampaignsAsync(BaseListOptions? options = null) {
			try {
				var parameters = new Dictionary<string, object>
				{
					{ "limit", options?.Limit ?? 25 },
					{ "page", options?.Page ?? 1 }
				};
				if (!string.IsNullOrEmpty(options?.Search)) {
					var searchArray = JsonSerializer.Deserialize<List<Dictionary<string, string>>>(options.Search);
					if (searchArray != null) {
						foreach (var searchItem in searchArray) {
							foreach (var kvp in searchItem) {
								parameters.Add(kvp.Key, kvp.Value);
							}
						}
					}
				}
				var query = BuildQueryString(parameters);
				return await GetCampaignsAsync("agent-hub/campaigns", query);
			} catch (Exception ex) {
				Console.WriteLine(ex);
				throw;
			}
		}

		/// Retrieve details of a specific campaign by ID.
		private async Task<BaseResponse?> ReceiveCampaignDetailsAsync(string campaignId, string type = "Campaign") {
			try {
				campaignId = campaignId.StartsWith("cmp_") ? campaignId.Substring(4) : campaignId;
				var url = $"agent-hub/campaigns/{campaignId}";
				var response = await _httpClient.GetAsync(url);
				response.EnsureSuccessStatusCode();

				var responseContent = await response.Content.ReadAsStringAsync();
				if (!response.IsSuccessStatusCode) {
					var errorData = JsonSerializer.Deserialize<Dictionary<string, object>>(responseContent);
					Console.WriteLine($"Error details: {JsonSerializer.Serialize(errorData)}");
					throw new InvalidOperationException($"HTTP error {response.StatusCode}: {responseContent}");
				}

				if (string.IsNullOrWhiteSpace(responseContent)) {
					throw new InvalidOperationException("Response body is empty.");
				}

				var data = JsonSerializer.Deserialize<Dictionary<string, object>>(responseContent);
				if (data != null && data.TryGetValue("data", out var dataValue) && dataValue is JsonElement dataElement) {
					var dataDict = JsonSerializer.Deserialize<Dictionary<string, object>>(dataElement.GetRawText());
					if (dataDict != null) {
						return TransformJsonRawObject(dataDict, dataElement.GetRawText(), type);
					}
				}
				throw new InvalidOperationException("Response data is invalid or missing.");
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

		// Update an existing campaign.
		private async Task<BaseResponse> UpdateCampaignAsync(string campaignId, SplitCampaignRequestData updatedData) {
			campaignId = campaignId.StartsWith("cmp_") ? campaignId.Substring(4) : campaignId;
			return await UpdateCampaign($"agent-hub/campaigns/{campaignId}", updatedData);
		}

		// Generic HTTP request methods
		private async Task<BaseResponse?> CreateSplitCampaign(string url, SplitCampaignRequestData customerData, string type = "Campaign") {
			try {
				var content = new StringContent(customerData.ToJson(), Encoding.UTF8, "application/json");
				var response = await _httpClient.PostAsync(url, content);
				response.EnsureSuccessStatusCode();
				var responseContent = await response.Content.ReadAsStringAsync();
				Console.WriteLine($"Response status code: {response.StatusCode}");
				if (!response.IsSuccessStatusCode) {
					var errorData = JsonSerializer.Deserialize<Dictionary<string, object>>(responseContent);
					Console.WriteLine($"Error details: {JsonSerializer.Serialize(errorData)}");
					throw new InvalidOperationException($"HTTP error {response.StatusCode}: {responseContent}");
				}

				if (string.IsNullOrWhiteSpace(responseContent)) {
					throw new InvalidOperationException("Response body is empty.");
				}

				var data = JsonSerializer.Deserialize<Dictionary<string, object>>(responseContent);
				if (data != null && data.TryGetValue("data", out var dataValue) && dataValue is JsonElement dataElement) {
					var dataDict = JsonSerializer.Deserialize<Dictionary<string, object>>(dataElement.GetRawText());
					if (dataDict != null) {
						return TransformJsonRawObject(dataDict, dataElement.GetRawText(), type);
					}
				}
				throw new InvalidOperationException("Response data is invalid or missing.");
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

		private async Task<ListBaseResponse> GetCampaignsAsync(string url, string? queryParams, string type = "Campaign") {
			try {
				if (queryParams != null) {
					url = $"{url}?{queryParams}";
				}

				var response = await _httpClient.GetAsync(url);
				response.EnsureSuccessStatusCode();
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
				var jsonCampaigns = dataElement.Deserialize<List<Dictionary<string, object>>>();
				List<BaseResponse?>? campaigns = new List<BaseResponse?>();
				if (jsonCampaigns != null) {
					for (int i = 0; i < jsonCampaigns.Count; i++) {
						var campaign = TransformJsonRawObject(jsonCampaigns[i], JsonSerializer.Serialize(jsonCampaigns[i]), type);
						campaigns?.Add(campaign);
					}
				}

				return new CampaignListResponse {
					Data = campaigns,
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

		private async Task<BaseResponse?> UpdateCampaign(string url, SplitCampaignRequestData campaignData, string type = "Campaign") {
			try {
				Console.WriteLine($"Campaign Data: {campaignData.ToJson()}");
				var content = new StringContent(campaignData.ToJson(), Encoding.UTF8, "application/json");
				var request = new HttpRequestMessage(HttpMethod.Patch, url) { Content = content };
				var response = await _httpClient.SendAsync(request);
				response.EnsureSuccessStatusCode();
				var responseContent = await response.Content.ReadAsStringAsync();
				Console.WriteLine($"Response status code: {response.StatusCode}");
				if (!response.IsSuccessStatusCode) {
					var errorData = JsonSerializer.Deserialize<Dictionary<string, object>>(responseContent);
					Console.WriteLine($"Error details: {JsonSerializer.Serialize(errorData)}");
					throw new InvalidOperationException($"HTTP error {response.StatusCode}: {responseContent}");
				}

				if (string.IsNullOrWhiteSpace(responseContent)) {
					throw new InvalidOperationException("Response body is empty.");
				}

				var data = JsonSerializer.Deserialize<Dictionary<string, object>>(responseContent);
				if (data != null && data.TryGetValue("data", out var dataValue) && dataValue is JsonElement dataElement) {
					var dataDict = JsonSerializer.Deserialize<Dictionary<string, object>>(dataElement.GetRawText());
					if (dataDict != null) {
						return TransformJsonRawObject(dataDict, dataElement.GetRawText(), type);
					}
				}
				throw new InvalidOperationException("Response data is invalid or missing.");
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

		private async Task<ListBaseResponse> GetAllAccounts(string url, string? queryParams, string type = "MyAccount") {
			try {
				if (queryParams != null) {
					url = $"{url}?{queryParams}";
				}

				var response = await _httpClient.GetAsync(url);
				response.EnsureSuccessStatusCode();
				var responseBody = await response.Content.ReadAsStringAsync();
				var evidenceList = JsonSerializer.Deserialize<List<object>>(responseBody);
				var wrappedObject = new { data = evidenceList };
				responseBody = JsonSerializer.Serialize(wrappedObject, new JsonSerializerOptions { WriteIndented = true });

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
				var jsonAccounts = dataElement.Deserialize<List<Dictionary<string, object>>>();
				List<BaseResponse?>? accounts = new List<BaseResponse?>();
				if (jsonAccounts != null) {
					for (int i = 0; i < jsonAccounts.Count; i++) {
						var account = TransformJsonRawObject(jsonAccounts[i], JsonSerializer.Serialize(jsonAccounts[i]), type);
						accounts?.Add(account);
					}
				}

				return new CampaignListResponse {
					Data = accounts,
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
	}
}
