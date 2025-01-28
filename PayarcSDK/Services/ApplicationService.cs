using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Newtonsoft.Json.Linq;

namespace PayarcSDK.Services {
	public class ApplicationService {
		private readonly HttpClient _httpClient;

		public ApplicationService(HttpClient httpClient) {
			_httpClient = httpClient;
		}

		public async Task<JObject> Create(JObject applicant) {
			return await AddLeadAsync(applicant);
		}

		public async Task<JObject> List(Dictionary<string, string> queryParams = null) {
			return await GetApplyAppsAsync(queryParams);
		}

		public async Task<JObject> Retrieve(string applicantId) {
			return await RetrieveApplicantAsync(applicantId);
		}

		public async Task<JObject> Update(string applicantId, JObject newData) {
			return await UpdateApplicantAsync(applicantId, newData);
		}

		public async Task<JObject> Delete(string applicantId) {
			return await DeleteApplicantAsync(applicantId);
		}

		public async Task<JObject> AddDocument(string applicantId, JObject document) {
			return await AddApplicantDocumentAsync(applicantId, document);
		}

		public async Task<JObject> Submit(string applicantId) {
			return await SubmitApplicantForSignatureAsync(applicantId);
		}

		public async Task<JObject> DeleteDocument(string documentId) {
			return await DeleteApplicantDocumentAsync(documentId);
		}

		public async Task<JObject> ListSubAgents() {
			return await GetSubAgentsAsync();
		}

		private async Task<JObject> AddLeadAsync(JObject applicant) {
			if (applicant["agentId"] != null && applicant["agentId"].ToString().StartsWith("usr_")) {
				applicant["agentId"] = applicant["agentId"].ToString().Substring(4);
			}

			return await PostAsync("agent-hub/apply/add-lead", applicant);
		}

		private async Task<JObject> GetApplyAppsAsync(Dictionary<string, string> queryParams = null) {
			return await GetAsync("agent-hub/apply-apps", queryParams);
		}

		private async Task<JObject> RetrieveApplicantAsync(string applicantId) {
			if (applicantId.StartsWith("appl_")) {
				applicantId = applicantId.Substring(5);
			}

			var applicantResponse = await GetAsync($"agent-hub/apply-apps/{applicantId}");
			var docsResponse = await GetAsync($"agent-hub/apply-documents/{applicantId}",
				new Dictionary<string, string> { { "limit", "0" } });

			applicantResponse["Documents"] = docsResponse["data"];
			return applicantResponse;
		}

		private async Task<JObject> UpdateApplicantAsync(string applicantId, JObject newData) {
			if (applicantId.StartsWith("appl_")) {
				applicantId = applicantId.Substring(5);
			}

			newData.Merge(JObject.FromObject(new {
				bank_account_type = "01",
				slugId = "financial_information",
				skipGIACT = true
			}));

			var response = await PatchAsync($"agent-hub/apply-apps/{applicantId}", newData);

			if (response != null && response.Value<int>("status") == 200) {
				return await RetrieveApplicantAsync(applicantId);
			}

			return response;
		}

		private async Task<JObject> DeleteApplicantAsync(string applicantId) {
			if (applicantId.StartsWith("appl_")) {
				applicantId = applicantId.Substring(5);
			}

			var data = new JObject { { "MerchantCode", applicantId } };
			return await PostAsync("agent-hub/apply/delete-lead", data);
		}

		private async Task<JObject> AddApplicantDocumentAsync(string applicantId, JObject document) {
			if (applicantId.StartsWith("appl_")) {
				applicantId = applicantId.Substring(5);
			}

			var requestBody = new JObject
			{
				{ "MerchantCode", applicantId },
				{ "MerchantDocuments", new JArray(document) }
			};

			return await PostAsync("agent-hub/apply/add-documents", requestBody);
		}

		private async Task<JObject> GetSubAgentsAsync() {
			return await GetAsync("agent-hub/sub-agents");
		}

		private async Task<JObject> DeleteApplicantDocumentAsync(string documentId) {
			if (documentId.StartsWith("doc_")) {
				documentId = documentId.Substring(4);
			}

			var requestBody = new JObject
			{
				{ "MerchantDocuments", new JArray(new JObject { { "DocumentCode", documentId } }) }
			};

			return await PostAsync("agent-hub/apply/delete-documents", requestBody);
		}

		private async Task<JObject> SubmitApplicantForSignatureAsync(string applicantId) {
			if (applicantId.StartsWith("appl_")) {
				applicantId = applicantId.Substring(5);
			}

			var requestBody = new JObject
			{
				{ "MerchantCode", applicantId }
			};

			return await PostAsync("agent-hub/apply/submit-for-signature", requestBody);
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

		private async Task<JObject> PatchAsync(string url, JObject data) {
			var content = new StringContent(data.ToString(), Encoding.UTF8, "application/json");
			var request = new HttpRequestMessage(HttpMethod.Patch, url) { Content = content };
			var response = await _httpClient.SendAsync(request);
			response.EnsureSuccessStatusCode();
			var responseContent = await response.Content.ReadAsStringAsync();
			return JObject.Parse(responseContent);
		}
	}
}
