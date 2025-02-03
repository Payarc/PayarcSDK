using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using AnyOfTypes;
using Newtonsoft.Json.Linq;
using PayarcSDK.Entities;
using PayarcSDK.Entities.ApplicationService;
using PayarcSDK.Entities.CustomerService;

namespace PayarcSDK.Services {
	public class ApplicationService : CommonServices {
		private readonly HttpClient _httpClient;

		public ApplicationService(HttpClient httpClient) : base(httpClient) {
			_httpClient = httpClient;
		}

		public async Task<BaseResponse> Create(ApplicationInfoData applicant) {
			return await AddLeadAsync(applicant);
		}

		public async Task<ListBaseResponse> List(OptionsData options) {
			return await ListApplyAppsAsync(options);
		}

		public async Task<BaseResponse> Retrieve(string applicantId) {
			return await RetrieveApplicantAsync(applicantId);
		}

		public async Task<BaseResponse> Update(AnyOf<string?, ApplicationResponseData> application, ApplicationInfoData newData) {
			string? applicantId = string.Empty;
			applicantId = application.IsSecond ? application.Second.ObjectId : application.First;
			return await UpdateApplicantAsync(applicantId, newData);
		}

		public async Task<BaseResponse> Delete(string applicantId) {
			return await DeleteApplicantAsync(applicantId);
		}

		public async Task<BaseResponse> AddDocument(string applicantId, List<MerchantDocument> merchantDocuments) {
			return await AddApplicantDocumentAsync(applicantId, merchantDocuments);
		}

		public async Task<BaseResponse> Submit(string applicantId) {
			return await SubmitApplicantForSignatureAsync(applicantId);
		}

		public async Task<BaseResponse> DeleteDocument(string applicantId, string documentId) {
			return await DeleteApplicantDocumentAsync(applicantId, documentId);
		}

		public async Task<ListBaseResponse> ListSubAgents(OptionsData options) {
			return await ListSubAgentsAsync(options);
		}

		private async Task<BaseResponse> AddLeadAsync(ApplicationInfoData applicant) {
			if (applicant.agentId != null && applicant.agentId.ToString().StartsWith("usr_")) {
				applicant.agentId = applicant.agentId.ToString().Substring(4);
			}

			return await CreateApplicationAsync("agent-hub/apply/add-lead", applicant);
		}

		private async Task<ListBaseResponse> ListApplyAppsAsync(OptionsData options = null) {
			try {
				var parameters = new Dictionary<string, object>
				{
					{ "limit", options.Limit ?? 25 },
					{ "page", options.Page ?? 1 }
				};
				if (!string.IsNullOrEmpty(options.Search)) {
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
				return await GetApplicationListAsync("agent-hub/apply-apps", query);
			} catch (Exception ex) {
				Console.WriteLine(ex);
				throw;
			}
		}

		private async Task<BaseResponse> RetrieveApplicantAsync(string applicantId) {
			try {
				applicantId = applicantId.StartsWith("appl_") ? applicantId.Substring(5) : applicantId;
				var applicantResponse = await GetApplyAppAsync($"agent-hub/apply-apps/{applicantId}", $"agent-hub/apply-documents/{applicantId}");
				return applicantResponse;
			} catch (Exception ex) {
				Console.WriteLine(ex);
				throw;
			}
		}

		private async Task<BaseResponse> UpdateApplicantAsync(string applicantId, ApplicationInfoData newData) {
			applicantId = applicantId.StartsWith("appl_") ? applicantId.Substring(5) : applicantId;
			//newData.Lead.BankAccountType = "01";
			//newData.Lead.SlugId = "financial_information";
			//newData.Lead.SkipGiact = true;

			var response = await UpdateApplicant($"agent-hub/apply/lead/{applicantId}/", newData);

			if (response != null) {
				return await RetrieveApplicantAsync(applicantId);
			}

			return response;
		}

		private async Task<BaseResponse> DeleteApplicantAsync(string applicantId) {
			applicantId = applicantId.StartsWith("appl_") ? applicantId.Substring(5) : applicantId;

			var parameters = new Dictionary<string, object> {
				{ "MerchantCode", applicantId }
			};
			var query = BuildQueryString(parameters);
			return await DeleteLeadAsync("agent-hub/apply/delete-lead", query);
		}

		private async Task<BaseResponse> AddApplicantDocumentAsync(string applicantId, List<MerchantDocument> merchantDocuments) {
			applicantId = applicantId.StartsWith("appl_") ? applicantId.Substring(5) : applicantId;
			DocumentData documentData = new DocumentData() {
				MerchantCode = applicantId,
				MerchantDocuments = merchantDocuments
			};
			return await AddDocumentAsync("agent-hub/apply/add-documents", documentData);
		}

		private async Task<ListBaseResponse> ListSubAgentsAsync(OptionsData options = null) {
            try {
                var parameters = new Dictionary<string, object>
                {
                    { "limit", options.Limit ?? 25 },
                    { "page", options.Page ?? 1 }
                };
                if (!string.IsNullOrEmpty(options.Search)) {
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
                return await GetListAubAgentsAsync("agent-hub/sub-agents", query);
            }
            catch (Exception ex) {
                Console.WriteLine(ex);
                throw;
            }
        }

		private async Task<BaseResponse> DeleteApplicantDocumentAsync(string applicantId, string documentId) {
			applicantId = applicantId.StartsWith("appl_") ? applicantId.Substring(5) : applicantId;
			documentId = documentId.StartsWith("doc_") ? documentId.Substring(4) : documentId;
			DocumentData documentData = new DocumentData() {
				MerchantCode = applicantId,
				MerchantDocuments = new List<MerchantDocument> {
					new MerchantDocument {
						DocumentCode = documentId
					}
				}
			};

			return await DeleteDocumentAsync("agent-hub/apply/delete-documents", documentData);
		}

		private async Task<BaseResponse> SubmitApplicantForSignatureAsync(string applicantId) {
			applicantId = applicantId.StartsWith("appl_") ? applicantId.Substring(5) : applicantId;

			var requestBody = new JObject
			{
				{ "MerchantCode", applicantId }
			};

			ApplicationInfoData applicationInfoData = new ApplicationInfoData();

			return await SubmitForSignatureAsync("agent-hub/apply/submit-for-signature", applicationInfoData);
		}

		// Generic HTTP request helper methods
		private async Task<ListBaseResponse> GetApplicationListAsync(string url, string? queryParams = null, string type = "ApplyApp") {
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
				var jsonApplications = dataElement.Deserialize<List<Dictionary<string, object>>>();
				List<BaseResponse?>? applications = new List<BaseResponse?>();
				if (jsonApplications != null) {
					for (int i = 0; i < jsonApplications.Count; i++) {
						var application = TransformJsonRawObject(jsonApplications[i], JsonSerializer.Serialize(jsonApplications[i]), type);
						applications?.Add(application);
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

				return new ApplicationListResponse {
					Data = applications,
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

		// Generic HTTP request helper methods
		private async Task<ListBaseResponse> GetListAubAgentsAsync(string url, string? queryParams = null, string type = "User") {
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
				var jsonSubAgents = dataElement.Deserialize<List<Dictionary<string, object>>>();
				List<BaseResponse?>? subAgents = new List<BaseResponse?>();
				if (jsonSubAgents != null) {
					for (int i = 0; i < jsonSubAgents.Count; i++) {
						var subAgent = TransformJsonRawObject(jsonSubAgents[i], JsonSerializer.Serialize(jsonSubAgents[i]), type);
						subAgents?.Add(subAgent);
					}
				}

				return new ApplicationListResponse {
					Data = subAgents,
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

		// Generic HTTP request helper methods
		private async Task<BaseResponse> GetApplyAppAsync(string appUrl, string docUrl, string? queryParams = null, string type = "ApplyApp") {
			try {
				if (queryParams != null) {
					appUrl = $"{appUrl}?{queryParams}";
				}

				var response = await _httpClient.GetAsync(appUrl);
				response.EnsureSuccessStatusCode();
				var responseApplicationBody = await response.Content.ReadAsStringAsync();
				Console.WriteLine($"Response status code: {response.StatusCode}");
				if (!response.IsSuccessStatusCode) {
					var errorData = JsonSerializer.Deserialize<Dictionary<string, object>>(responseApplicationBody);
					Console.WriteLine($"Error details: {JsonSerializer.Serialize(errorData)}");
					throw new InvalidOperationException($"HTTP error {response.StatusCode}: {responseApplicationBody}");
				}

				if (string.IsNullOrWhiteSpace(responseApplicationBody)) {
					throw new InvalidOperationException("Response body is empty.");
				}

				//Attach the documents to the application
				if (queryParams != null) {
					docUrl = $"{docUrl}?{queryParams}";
				}

				var responseDocuments = await _httpClient.GetAsync(docUrl);
				responseDocuments.EnsureSuccessStatusCode();
				var responseDocumentsBody = await responseDocuments.Content.ReadAsStringAsync();
				Console.WriteLine($"Response status code: {responseDocuments.StatusCode}");
				if (!responseDocuments.IsSuccessStatusCode) {
					var errorData = JsonSerializer.Deserialize<Dictionary<string, object>>(responseDocumentsBody);
					Console.WriteLine($"Error details: {JsonSerializer.Serialize(errorData)}");
					throw new InvalidOperationException($"HTTP error {responseDocuments.StatusCode}: {responseDocumentsBody}");
				}

				if (string.IsNullOrWhiteSpace(responseDocumentsBody)) {
					throw new InvalidOperationException("Response body is empty.");
				}

				// Deserialize both JSONs
				using var documentJson = JsonDocument.Parse(responseDocumentsBody);
				using var applicationJson = JsonDocument.Parse(responseApplicationBody);

				var documentDataList = documentJson.RootElement.GetProperty("data"); // Extract "data" array from documentJson
				var appDataRoot = applicationJson.RootElement.Clone(); // Clone applicationJson structure

				// Ensure "data" is an object before modifying
				if (appDataRoot.TryGetProperty("data", out JsonElement applicationDataElement) && applicationDataElement.ValueKind == JsonValueKind.Object) {
					var updatedApplicationData = new Dictionary<string, JsonElement>();

					// Copy all existing properties from "data"
					foreach (var property in applicationDataElement.EnumerateObject()) {
						updatedApplicationData[property.Name] = property.Value;
					}

					// Add "Documents" key containing the extracted "data" from documentJson
					updatedApplicationData["Documents"] = documentDataList;

					// Create final merged JSON object
					var mergedJson = new Dictionary<string, object> {
						["data"] = updatedApplicationData,
						["meta"] = appDataRoot.GetProperty("meta") // Keeping "meta" unchanged
					};

					// Serialize back to JSON
					responseApplicationBody = JsonSerializer.Serialize(mergedJson);
				} else {
					Console.WriteLine("Error: The 'data' field is not an object.");
				}

				var responseData = JsonSerializer.Deserialize<Dictionary<string, object>>(responseApplicationBody);
				if (responseData == null || !responseData.TryGetValue("data", out var dataValue) ||
					!(dataValue is JsonElement dataElement)) {
					throw new InvalidOperationException("Response data is invalid or missing.");
				}

				var rawData = dataElement.GetRawText();
				var jsonApplication = dataElement.Deserialize<Dictionary<string, object>>();
				return TransformJsonRawObject(jsonApplication, JsonSerializer.Serialize(jsonApplication), type);

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

		// Generic HTTP request helper methods
		private async Task<ListBaseResponse> GetApplyDocumentsAsync(string url, string? queryParams = null, string type = "ApplyDocuments") {
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
				var jsonDocuments = dataElement.Deserialize<List<Dictionary<string, object>>>();
				List<BaseResponse?>? documents = new List<BaseResponse?>();
				if (jsonDocuments != null) {
					for (int i = 0; i < jsonDocuments.Count; i++) {
						var document = TransformJsonRawObject(jsonDocuments[i], JsonSerializer.Serialize(jsonDocuments[i]), type);
						documents?.Add(document);
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

				return new ApplicationListResponse {
					Data = documents,
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

		private async Task<BaseResponse> CreateApplicationAsync(string url, ApplicationInfoData applicationData, string type = "MerchantCode") {
			try {
				var content = new StringContent(applicationData.ToJson(), Encoding.UTF8, "application/json");
				Console.WriteLine($"ApplicationInfoData : {applicationData.ToJson()}");
				var response = await _httpClient.PostAsync(url, content);
				response.EnsureSuccessStatusCode();
				var responseContent = await response.Content.ReadAsStringAsync();
				//return JObject.Parse(responseContent);
				Console.WriteLine($"Response status code: {response.StatusCode}");
				if (!response.IsSuccessStatusCode) {
					var errorData = JsonSerializer.Deserialize<Dictionary<string, object>>(responseContent);
					Console.WriteLine($"Error details: {JsonSerializer.Serialize(errorData)}");
					throw new InvalidOperationException($"HTTP error {response.StatusCode}: {responseContent}");
				}

				if (string.IsNullOrWhiteSpace(responseContent)) {
					throw new InvalidOperationException("Response body is empty.");
				}

				var responseData = JsonSerializer.Deserialize<Dictionary<string, object>>(responseContent);
				if (responseData == null || !responseData.TryGetValue("MerchantCode", out var dataValue) ||
					!(dataValue is JsonElement dataElement)) {
					throw new InvalidOperationException("Response data is invalid or missing.");
				}

				var rawData = dataElement.GetRawText();
				return TransformJsonRawObject(responseData, rawData, type);
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

		private async Task<BaseResponse> AddDocumentAsync(string url, DocumentData documentData, string type = "MerchantCode") {
			try {
				Console.WriteLine($"Document Data : {documentData.ToJson()}");
				var content = new StringContent(documentData.ToJson(), Encoding.UTF8, "application/json");
				var response = await _httpClient.PostAsync(url, content);
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

				var responseData = JsonSerializer.Deserialize<Dictionary<string, object>>(responseContent);
				if (responseData == null || !responseData.TryGetValue("MerchantCode", out var dataValue) ||
					!(dataValue is JsonElement dataElement)) {
					throw new InvalidOperationException("Response data is invalid or missing.");
				}

				var rawData = dataElement.GetRawText();
				return TransformJsonRawObject(responseData, JsonSerializer.Serialize(responseData), type);
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

		private async Task<BaseResponse> DeleteDocumentAsync(string url, DocumentData documentData, string type = "DocumentCode") {
			try {
				Console.WriteLine($"Document Data : {documentData.ToJson()}");
				var content = new StringContent(documentData.ToJson(), Encoding.UTF8, "application/json");
				// Create DELETE request with body
				using var request = new HttpRequestMessage(HttpMethod.Delete, url) {
					Content = content
				};

				// Send the DELETE request
				var response = await _httpClient.SendAsync(request);
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

				var responseData = JsonSerializer.Deserialize<Dictionary<string, object>>(responseContent);
				if (responseData == null || !responseData.TryGetValue("MerchantDocuments", out var dataValue) ||
					!(dataValue is JsonElement dataElement)) {
					throw new InvalidOperationException("Response data is invalid or missing.");
				}

				var rawData = dataElement.GetRawText();
				return TransformJsonRawObject(responseData, JsonSerializer.Serialize(responseData), type);
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

		private async Task<BaseResponse> SubmitForSignatureAsync(string url, ApplicationInfoData applicationData, string type = "Application") {
			try {
				var content = new StringContent(applicationData.ToJson(), Encoding.UTF8, "application/json");
				var response = await _httpClient.PostAsync(url, content);
				response.EnsureSuccessStatusCode();
				var responseContent = await response.Content.ReadAsStringAsync();
				//return JObject.Parse(responseContent);
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

		private async Task<BaseResponse> DeleteLeadAsync(string url, string? queryParams, string type = "MerchantCode") {
			try {
				if (queryParams != null) {
					url = $"{url}?{queryParams}";
				}
				var response = await _httpClient.DeleteAsync(url);
				response.EnsureSuccessStatusCode();
				var responseContent = await response.Content.ReadAsStringAsync();
				//return JObject.Parse(responseContent);
				Console.WriteLine($"Response status code: {response.StatusCode}");
				if (!response.IsSuccessStatusCode) {
					var errorData = JsonSerializer.Deserialize<Dictionary<string, object>>(responseContent);
					Console.WriteLine($"Error details: {JsonSerializer.Serialize(errorData)}");
					throw new InvalidOperationException($"HTTP error {response.StatusCode}: {responseContent}");
				}

				if (string.IsNullOrWhiteSpace(responseContent)) {
					throw new InvalidOperationException("Response body is empty.");
				}

				var responseData = JsonSerializer.Deserialize<Dictionary<string, object>>(responseContent);
				if (responseData == null || !responseData.TryGetValue("MerchantCode", out var dataValue) ||
					!(dataValue is JsonElement dataElement)) {
					throw new InvalidOperationException("Response data is invalid or missing.");
				}

				var rawData = dataElement.GetRawText();
				//var jsonApplication = dataElement.Deserialize<Dictionary<string, object>>();
				return TransformJsonRawObject(responseData, rawData, type);
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

		private async Task<BaseResponse> UpdateApplicant(string url, ApplicationInfoData applicationData, string type = "ApplyApp") {
			try {
				Console.WriteLine($"Customer Data: {applicationData.ToJson()}");
				var content = new StringContent(applicationData.ToJson(), Encoding.UTF8, "application/json");
				var request = new HttpRequestMessage(HttpMethod.Patch, url) { Content = content };
				var response = await _httpClient.SendAsync(request);
				response.EnsureSuccessStatusCode();
				var responseContent = await response.Content.ReadAsStringAsync();
				Console.WriteLine($"Response status code: {response.StatusCode}");
				//return JObject.Parse(responseContent);
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
	}
}
