using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Newtonsoft.Json.Linq;

namespace PayarcSDK.Services {
	public class DisputeService {
		private readonly HttpClient _httpClient;

		public DisputeService(HttpClient httpClient) {
			_httpClient = httpClient;
		}

		public async Task<JObject> List(Dictionary<string, string> queryParams = null) {
			return await ListCasesAsync(queryParams);
		}

		public async Task<JObject> Retrieve(string disputeId) {
			return await GetCaseAsync(disputeId);
		}

		public async Task<JObject> AddDocument(string disputeId, JObject documentParams) {
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

			return await GetAsync("cases", queryParams);
		}

		// Retrieve a specific dispute case
		private async Task<JObject> GetCaseAsync(string disputeId) {
			if (disputeId.StartsWith("dis_")) {
				disputeId = disputeId.Substring(4);
			}

			return await GetAsync($"cases/{disputeId}");
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
				fileContent.Headers.ContentType = new MediaTypeHeaderValue(mimeType);

				content.Add(fileContent, "file", "filename1.png");
			}

			if (documentParams.TryGetValue("text", out var text)) {
				var stringContent = new StringContent(text.ToString());
				content.Add(stringContent, "text");
			}

			// Submit evidence
			var evidenceResponse = await PostRawAsync($"cases/{disputeId}/evidence", content);

			// Submit case with a message
			var message = documentParams.Value<string>("message") ?? "Case number#: xxxxxxxx, submitted by SDK";
			var submitBody = JObject.FromObject(new { message });
			var submitResponse = await PostAsync($"cases/{disputeId}/submit", submitBody);

			return JObject.FromObject(new {
				EvidenceResponse = evidenceResponse,
				SubmitResponse = submitResponse
			});
		}

		// Generic HTTP request helper methods
		private async Task<JObject> GetAsync(string url, Dictionary<string, string> queryParams = null) {
			if (queryParams != null) {
				var query = string.Join("&", queryParams.Select(p => $"{Uri.EscapeDataString(p.Key)}={Uri.EscapeDataString(p.Value)}"));
				url = $"{url}?{query}";
			}

			var response = await _httpClient.GetAsync(url);
			response.EnsureSuccessStatusCode();
			var responseContent = await response.Content.ReadAsStringAsync();
			return JObject.Parse(responseContent);
		}

		private async Task<JObject> PostAsync(string url, JObject data) {
			var content = new StringContent(data.ToString(), Encoding.UTF8, "application/json");
			var response = await _httpClient.PostAsync(url, content);
			response.EnsureSuccessStatusCode();
			var responseContent = await response.Content.ReadAsStringAsync();
			return JObject.Parse(responseContent);
		}

		private async Task<JObject> PostRawAsync(string url, HttpContent content) {
			var response = await _httpClient.PostAsync(url, content);
			response.EnsureSuccessStatusCode();
			var responseContent = await response.Content.ReadAsStringAsync();
			return JObject.Parse(responseContent);
		}
	}
}
