using Newtonsoft.Json.Linq;
using PayarcSDK.Http;

namespace PayarcSDK.Services {
	public class DisputeService {
		private readonly ApiClient _apiClient;

		public DisputeService(ApiClient apiClient) {
			_apiClient = apiClient;
		}

		public async Task<JObject> list(Dictionary<string, string> queryParams = null) {
			return await ListCasesAsync(queryParams);
		}

		public async Task<JObject> retrieve(string disputeId) {
			return await GetCaseAsync(disputeId);
		}

		public async Task<JObject> addDocument(string disputeId, JObject documentParams) {
			return await AddDocumentCaseAsync(disputeId, documentParams);
		}

		// List disputes with optional filters
		private async Task<JObject> ListCasesAsync(Dictionary<string, string> queryParams = null) {
			if (queryParams == null) {
				var currentDate = DateTime.UtcNow;
				var tomorrowDate = currentDate.AddDays(1).ToString("yyyy-MM-dd");
				var lastMonthDate = currentDate.AddMonths(-1).ToString("yyyy-MM-dd");

				queryParams = new Dictionary<string, string>
				{
					{ "report_date[gte]", lastMonthDate },
					{ "report_date[lte]", tomorrowDate }
				};
			}

			return await _apiClient.GetAsync("cases", queryParams);
		}

		// Retrieve a specific dispute case
		private async Task<JObject> GetCaseAsync(string disputeId) {
			if (disputeId.StartsWith("dis_")) {
				disputeId = disputeId.Substring(4);
			}

			return await _apiClient.GetAsync($"cases/{disputeId}");
		}

		// Add a document to a dispute case
		private async Task<JObject> AddDocumentCaseAsync(string disputeId, JObject documentParams) {
			if (disputeId.StartsWith("dis_")) {
				disputeId = disputeId.Substring(4);
			}

			// Prepare form-data content
			var boundary = "----WebKitFormBoundary3OdUODzy6DLxDNt8";
			var content = new MultipartFormDataContent(boundary);

			if (documentParams.TryGetValue("DocumentDataBase64", out var base64Data)) {
				var binaryData = Convert.FromBase64String(base64Data.ToString());
				var fileContent = new ByteArrayContent(binaryData);

				var mimeType = documentParams.Value<string>("mimeType") ?? "application/pdf";
				fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(mimeType);

				content.Add(fileContent, "file", "filename1.png");
			}

			if (documentParams.TryGetValue("text", out var text)) {
				var stringContent = new StringContent(text.ToString());
				content.Add(stringContent, "text");
			}

			// Submit evidence
			var response = await _apiClient.PostRawAsync($"cases/{disputeId}/evidence", content);

			// Submit case with a message
			var message = documentParams.Value<string>("message") ?? "Case number#: xxxxxxxx, submitted by SDK";
			var submitBody = JObject.FromObject(new { message });
			var submitResponse = await _apiClient.PostAsync($"cases/{disputeId}/submit", submitBody);

			return JObject.FromObject(new {
				EvidenceResponse = response,
				SubmitResponse = submitResponse
			});
		}
	}
}
