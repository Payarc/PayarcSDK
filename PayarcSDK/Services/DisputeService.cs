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

		//public async Task<JObject> AddDocument(string disputeId, JObject documentParams) {
		//	return await AddDocumentCaseAsync(disputeId, documentParams);
		//}

		// List disputes with optional filters
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

		//// Add a document to a dispute case
		//private async Task<JObject> AddDocumentCaseAsync(string disputeId, JObject documentParams) {
		//	if (disputeId.StartsWith("dis_")) {
		//		disputeId = disputeId.Substring(4);
		//	}

		//	// Prepare form-data content
		//	var boundary = "----WebKitFormBoundary3OdUODzy6DLxDNt8";
		//	var content = new MultipartFormDataContent(boundary);

		//	if (documentParams.TryGetValue("DocumentDataBase64", out var base64Data)) {
		//		var binaryData = Convert.FromBase64String(base64Data.ToString());
		//		var fileContent = new ByteArrayContent(binaryData);

		//		var mimeType = documentParams.Value<string>("mimeType") ?? "application/pdf";
		//		fileContent.Headers.ContentType = new MediaTypeHeaderValue(mimeType);

		//		content.Add(fileContent, "file", "filename1.png");
		//	}

		//	if (documentParams.TryGetValue("text", out var text)) {
		//		var stringContent = new StringContent(text.ToString());
		//		content.Add(stringContent, "text");
		//	}

		//	// Submit evidence
		//	var evidenceResponse = await PostRawAsync($"cases/{disputeId}/evidence", content);

		//	// Submit case with a message
		//	var message = documentParams.Value<string>("message") ?? "Case number#: xxxxxxxx, submitted by SDK";
		//	var submitBody = JObject.FromObject(new { message });
		//	var submitResponse = await PostAsync($"cases/{disputeId}/submit", submitBody);

		//	return JObject.FromObject(new {
		//		EvidenceResponse = evidenceResponse,
		//		SubmitResponse = submitResponse
		//	});
		//}

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
				var jsonApplications = dataElement.Deserialize<List<Dictionary<string, object>>>();
				List<BaseResponse?>? disputeCases = new List<BaseResponse?>();
				if (jsonApplications != null) {
					for (int i = 0; i < jsonApplications.Count; i++) {
						var disputeCase = TransformJsonRawObject(jsonApplications[i], JsonSerializer.Serialize(jsonApplications[i]), type);
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
				responseContent = primaryCase?.ToJsonString() ?? "{}";

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
