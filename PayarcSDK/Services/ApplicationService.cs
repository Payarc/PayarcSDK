using Newtonsoft.Json.Linq;
using PayarcSDK.Http;

namespace PayarcSDK.Services {
	public class ApplicationService {
		private readonly ApiClient _apiClient;

		public ApplicationService(ApiClient apiClient) {
			_apiClient = apiClient;
		}

		public async Task<JObject> create(JObject applicant) {
			return await AddLeadAsync(applicant);
		}

		public async Task<JObject> list(Dictionary<string, string> queryParams = null) {
			return await GetApplyAppsAsync(queryParams);
		}

		public async Task<JObject> retrieve(string applicantId) {
			return await RetrieveApplicantAsync(applicantId);
		}

		public async Task<JObject> update(string applicantId, JObject newData) {
			return await UpdateApplicantAsync(applicantId, newData);
		}
		public async Task<JObject> delete(string applicantId) {
			return await DeleteApplicantAsync(applicantId);
		}

		public async Task<JObject> addDocument(string applicantId, JObject document) {
			return await AddApplicantDocumentAsync(applicantId, document);
		}

		public async Task<JObject> submit(string applicantId) {
			return await SubmitApplicantForSignatureAsync(applicantId);
		}

		public async Task<JObject> deleteDocument(string documentId) {
			return await DeleteApplicantDocumentAsync(documentId);
		}

		public async Task<JObject> listSubAgents() {
			return await GetSubAgentsAsync();
		}

		private async Task<JObject> AddLeadAsync(JObject applicant) {
			if (applicant["agentId"] != null && applicant["agentId"].ToString().StartsWith("usr_")) {
				applicant["agentId"] = applicant["agentId"].ToString().Substring(4);
			}

			return await _apiClient.PostAsync("agent-hub/apply/add-lead", applicant);
		}

		private async Task<JObject> GetApplyAppsAsync(Dictionary<string, string> queryParams = null) {
			//var queryParams = new Dictionary<string, string> {
			//	{ "limit", "0" },
			//	{ "is_archived", "0" }
			//};
			return await _apiClient.GetAsync("agent-hub/apply-apps", queryParams);
		}

		private async Task<JObject> RetrieveApplicantAsync(string applicantId) {
			if (applicantId.StartsWith("appl_")) {
				applicantId = applicantId.Substring(5);
			}

			var applicantResponse = await _apiClient.GetAsync($"agent-hub/apply-apps/{applicantId}");
			var docsResponse = await _apiClient.GetAsync($"agent-hub/apply-documents/{applicantId}",
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

			var response = await _apiClient.PatchAsync($"agent-hub/apply-apps/{applicantId}", newData);

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
			return await _apiClient.PostAsync("agent-hub/apply/delete-lead", data);
		}

		private async Task<JObject> AddApplicantDocumentAsync(string applicantId, JObject document) {
			if (applicantId.StartsWith("appl_")) {
				applicantId = applicantId.Substring(5);
			}

			var requestBody = new JObject {
				{ "MerchantCode", applicantId },
				{ "MerchantDocuments", new JArray(document) }
			};

			return await _apiClient.PostAsync("agent-hub/apply/add-documents", requestBody);
		}

		private async Task<JObject> GetSubAgentsAsync() {
			return await _apiClient.GetAsync("agent-hub/sub-agents");
		}

		private async Task<JObject> DeleteApplicantDocumentAsync(string documentId) {
			if (documentId.StartsWith("doc_")) {
				documentId = documentId.Substring(4);
			}

			var requestBody = new JObject {
				{ "MerchantDocuments", new JArray(new JObject { { "DocumentCode", documentId } }) }
			};

			return await _apiClient.PostAsync("agent-hub/apply/delete-documents", requestBody);
		}

		private async Task<JObject> SubmitApplicantForSignatureAsync(string applicantId) {
			if (applicantId.StartsWith("appl_")) {
				applicantId = applicantId.Substring(5);
			}

			var requestBody = new JObject {
				{ "MerchantCode", applicantId }
			};

			return await _apiClient.PostAsync("agent-hub/apply/submit-for-signature", requestBody);
		}
	}
}
