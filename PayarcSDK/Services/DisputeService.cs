using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using Newtonsoft.Json.Linq;
using PayarcSDK.Entities;
using PayarcSDK.Entities.Dispute;

namespace PayarcSDK.Services {
	public class DisputeService : CommonServices {
		private readonly HttpClient _httpClient;

		public DisputeService(HttpClient httpClient) : base(httpClient) {
			_httpClient = httpClient;
		}

		public async Task<ListBaseResponse> List(OptionsData options) {
			return await ListCasesAsync(options);
		}

		public async Task<BaseResponse> Retrieve(string disputeId) {
			return await RetriveDisputeCaseAsync(disputeId);
		}

		private async Task<ListBaseResponse> ListCasesAsync(OptionsData options = null) {
			if (options == null) {
				var currentDate = DateTime.UtcNow;
				var tomorrowDate = currentDate.AddDays(1).ToString("yyyy-MM-dd");
				var lastMonthDate = currentDate.AddMonths(-1).ToString("yyyy-MM-dd");

				options = new OptionsData {
					Report_DateGTE = lastMonthDate,
					Report_DateLTE = tomorrowDate,
				};
			}

			var parameters = new Dictionary<string, object> {
				{ "report_date[gte]", options.Report_DateGTE },
				{ "report_date[lte]", options.Report_DateLTE }
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

			return await GetDisputeCasesAsync("cases", query);
		}

		public async Task<string> AddCaseDocumentAsync(string disputeId, DocumentParameters documentParameters) {
			try {
				disputeId = disputeId.StartsWith("dis_") ? disputeId.Substring(4) : disputeId;

				var boundary = "----WebKitFormBoundary3OdUODzy6DLxDNt8";
				using var multipartContent = new MultipartFormDataContent(boundary);

				if (!string.IsNullOrEmpty(documentParameters.DocumentDataBase64)) {
					byte[] fileBytes = Convert.FromBase64String(documentParameters.DocumentDataBase64);
					var byteArrayContent = new ByteArrayContent(fileBytes);
					byteArrayContent.Headers.ContentType = new MediaTypeHeaderValue(documentParameters.MimeType);
					multipartContent.Add(byteArrayContent, "file", "filename1.png");
				}

				var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"cases/{disputeId}/evidence") {
					Content = multipartContent
				};

				var response = await _httpClient.SendAsync(requestMessage);
				response.EnsureSuccessStatusCode();
				string responseData = await response.Content.ReadAsStringAsync();

				// Submit the case
				var submitContent = new StringContent(JsonSerializer.Serialize(new { documentParameters.Message }), Encoding.UTF8, "application/json");
				var submitRequestMessage = new HttpRequestMessage(HttpMethod.Post, $"cases/{disputeId}/submit") {
					Content = submitContent
				};

				var submitResponse = await _httpClient.SendAsync(submitRequestMessage);
				submitResponse.EnsureSuccessStatusCode();

				return responseData;
			} catch (HttpRequestException ex) {
				// Handle error appropriately in SDK (logging, wrapping exceptions, etc.)
				return $"Error: {ex.Message}";
			}
		}

		private async Task<ListBaseResponse> GetDisputeCasesAsync(string url, string? queryParams = null, string type = "Cases") {
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
				var jsonDicputeCases = dataElement.Deserialize<List<Dictionary<string, object>>>();
				List<BaseResponse?>? disputeCases = new List<BaseResponse?>();
				if (jsonDicputeCases != null) {
					for (int i = 0; i < jsonDicputeCases.Count; i++) {
						var disputeCase = TransformJsonRawObject(jsonDicputeCases[i], JsonSerializer.Serialize(jsonDicputeCases[i]), type);
						disputeCases?.Add(disputeCase);
					}
				}

				return new DisputeListResponse {
					Data = disputeCases,
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

		// Retrieve a specific dispute case
		private async Task<BaseResponse> RetriveDisputeCaseAsync(string disputeId, string type = "Cases") {
			try {
				disputeId = disputeId.StartsWith("dis_") ? disputeId.Substring(4) : disputeId;
				var url = $"cases/{disputeId}/";
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

				var jsonObject = JsonNode.Parse(responseContent);
				var primaryCase = jsonObject?["primary_case"];
				var primaryCaseFiles = primaryCase["data"]["file"];
				var primaryEvidences = primaryCase["data"]["evidence"];
				var primarySubmissions = primaryCase["data"]["case_submission"];
				primaryCase?["data"]?.AsObject().Remove("file");
				primaryCase?["data"]?.AsObject().Remove("evidence");
				primaryCase?["data"]?.AsObject().Remove("case_submission");
				responseContent = primaryCase?.ToJsonString() ?? "{}";
				var responseCaseFiles = primaryCaseFiles?.ToJsonString() ?? "[]";
				var responseEvidences = primaryEvidences?.ToJsonString() ?? "[]";
				var responseCaseSubmissions = primarySubmissions?.ToJsonString() ?? "[]";

				var responseCaseFilesData = JsonSerializer.Deserialize<Dictionary<string, object>>(responseCaseFiles);
				if (responseCaseFilesData == null || !responseCaseFilesData.TryGetValue("data", out var caseFilesDataValue) ||
					!(caseFilesDataValue is JsonElement caseFilesDataElement)) {
					throw new InvalidOperationException("Response data is invalid or missing.");
				}

				var caseFilesRawData = caseFilesDataElement.GetRawText();
				var jsonCaseFiles = caseFilesDataElement.Deserialize<List<Dictionary<string, object>>>();
				List<BaseResponse?>? disputeCaseFiles = new List<BaseResponse?>();
				if (primaryCaseFiles != null) {
					for (int i = 0; i < jsonCaseFiles.Count; i++) {
						var disputeCase = TransformJsonRawObject(jsonCaseFiles[i], JsonSerializer.Serialize(jsonCaseFiles[i]), "CaseFile");
						disputeCaseFiles?.Add(disputeCase);
					}
				}
				CaseFileListResponse listResponseCaseFiles = new CaseFileListResponse {
					Data = disputeCaseFiles,
					RawData = caseFilesRawData
				};

				var responseEvidencesData = JsonSerializer.Deserialize<Dictionary<string, object>>(responseEvidences);
				if (responseEvidencesData == null || !responseEvidencesData.TryGetValue("data", out var evidencesDataValue) ||
					!(evidencesDataValue is JsonElement evidencesDataElement)) {
					throw new InvalidOperationException("Response data is invalid or missing.");
				}

				var evidencesRawData = evidencesDataElement.GetRawText();
				var jsonEvidences = evidencesDataElement.Deserialize<List<Dictionary<string, object>>>();
				List<BaseResponse?>? disputeEvidences = new List<BaseResponse?>();
				if (primaryEvidences != null) {
					for (int i = 0; i < jsonEvidences.Count; i++) {
						var disputeEvidence = TransformJsonRawObject(jsonEvidences[i], JsonSerializer.Serialize(jsonEvidences[i]), "Evidence");
						disputeEvidences?.Add(disputeEvidence);
					}
				}
				EvidenceListResponse listResponseEvidences = new EvidenceListResponse {
					Data = disputeEvidences,
					RawData = evidencesRawData
				};

				var responseCaseSubmissionsData = JsonSerializer.Deserialize<Dictionary<string, object>>(responseCaseSubmissions);
				if (responseCaseSubmissionsData == null || !responseCaseSubmissionsData.TryGetValue("data", out var submissionsDataValue) ||
					!(submissionsDataValue is JsonElement caseSubmissionsDataElement)) {
					throw new InvalidOperationException("Response data is invalid or missing.");
				}

				var caseSubmissionsRawData = caseSubmissionsDataElement.GetRawText();
				var jsonCaseSubmissions = caseSubmissionsDataElement.Deserialize<List<Dictionary<string, object>>>();
				List<BaseResponse?>? disputeSubmissions = new List<BaseResponse?>();
				if (primarySubmissions != null) {
					for (int i = 0; i < jsonCaseSubmissions.Count; i++) {
						var disputeSubmission = TransformJsonRawObject(jsonCaseSubmissions[i], JsonSerializer.Serialize(jsonCaseSubmissions[i]), "CaseSubmission");
						disputeSubmissions?.Add(disputeSubmission);
					}
				}
				CaseSubmissionListResponse listResponseSubmissions = new CaseSubmissionListResponse {
					Data = disputeSubmissions,
					RawData = caseSubmissionsRawData
				};

				var data = JsonSerializer.Deserialize<Dictionary<string, object>>(responseContent);

				if (data != null && data.TryGetValue("data", out var dataValue) && dataValue is JsonElement dataElement) {
					var dataDict = JsonSerializer.Deserialize<Dictionary<string, object>>(dataElement.GetRawText());
					if (dataDict != null) {
						var disputeResponse = TransformJsonRawObject(dataDict, dataElement.GetRawText(), type);
						((DisputeCasesResponseData)disputeResponse).File = listResponseCaseFiles;
						((DisputeCasesResponseData)disputeResponse).Evidence = listResponseEvidences;
						((DisputeCasesResponseData)disputeResponse).CaseSubmission = listResponseSubmissions;
						return disputeResponse;
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
