using Newtonsoft.Json.Linq;
using PayarcSDK.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PayarcSDK.Services {
	public class ApplicationService {
		private readonly ApiClient _apiClient;

		public ApplicationService(ApiClient apiClient) {
			_apiClient = apiClient;
		}

		public async Task<JObject> AddLeadAsync(JObject applicant) {
			if (applicant["agentId"] != null && applicant["agentId"].ToString().StartsWith("usr_")) {
				applicant["agentId"] = applicant["agentId"].ToString().Substring(4);
			}

			return await _apiClient.PostAsync("agent-hub/apply/add-lead", applicant);
		}

		public async Task<JObject> GetApplyAppsAsync() {
			var queryParams = new Dictionary<string, string> {
				{ "limit", "0" },
				{ "is_archived", "0" }
			};
			return await _apiClient.GetAsync("agent-hub/apply-apps", queryParams);
		}

		public async Task<JObject> RetrieveApplicantAsync(string applicantId) {
			if (applicantId.StartsWith("appl_")) {
				applicantId = applicantId.Substring(5);
			}

			var applicantResponse = await _apiClient.GetAsync($"agent-hub/apply-apps/{applicantId}");
			var docsResponse = await _apiClient.GetAsync($"agent-hub/apply-documents/{applicantId}",
				new Dictionary<string, string> { { "limit", "0" } });

			applicantResponse["Documents"] = docsResponse["data"];
			return applicantResponse;
		}

		public async Task<JObject> UpdateApplicantAsync(string applicantId, JObject newData) {
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

		public async Task<JObject> DeleteApplicantAsync(string applicantId) {
			if (applicantId.StartsWith("appl_")) {
				applicantId = applicantId.Substring(5);
			}

			var data = new JObject { { "MerchantCode", applicantId } };
			return await _apiClient.PostAsync("agent-hub/apply/delete-lead", data);
		}

		public async Task<JObject> AddApplicantDocumentAsync(string applicantId, JObject document) {
			if (applicantId.StartsWith("appl_")) {
				applicantId = applicantId.Substring(5);
			}

			var requestBody = new JObject {
				{ "MerchantCode", applicantId },
				{ "MerchantDocuments", new JArray(document) }
			};

			return await _apiClient.PostAsync("agent-hub/apply/add-documents", requestBody);
		}

		public async Task<JObject> GetSubAgentsAsync() {
			return await _apiClient.GetAsync("agent-hub/sub-agents");
		}

		public async Task<JObject> DeleteApplicantDocumentAsync(string documentId) {
			if (documentId.StartsWith("doc_")) {
				documentId = documentId.Substring(4);
			}

			var requestBody = new JObject {
				{ "MerchantDocuments", new JArray(new JObject { { "DocumentCode", documentId } }) }
			};

			return await _apiClient.PostAsync("agent-hub/apply/delete-documents", requestBody);
		}

		public async Task<JObject> SubmitApplicantForSignatureAsync(string applicantId) {
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
