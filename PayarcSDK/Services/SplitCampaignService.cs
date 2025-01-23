using Newtonsoft.Json.Linq;
using PayarcSDK.Http;

namespace PayarcSDK.Services {
	public class SplitCampaignService {
		private readonly ApiClient _apiClient;

		public SplitCampaignService(ApiClient apiClient) {
			_apiClient = apiClient;
		}

		/// Create a new campaign.
		public async Task<JObject> CreateCampaignAsync(JObject campaignData) {
			try {
				return await _apiClient.PostAsync("agent-hub/campaigns", campaignData);
			} catch (Exception ex) {
				throw new Exception("Error creating campaign.", ex);
			}
		}

		/// Retrieve all campaigns.
		public async Task<JObject> GetAllCampaignsAsync() {
			try {
				var queryParams = new Dictionary<string, string>
				{
					{ "limit", "0" }
				};

				return await _apiClient.GetAsync("agent-hub/campaigns", queryParams);
			} catch (Exception ex) {
				throw new Exception("Error retrieving campaigns.", ex);
			}
		}

		/// Retrieve details of a specific campaign by ID.
		public async Task<JObject> GetCampaignDetailsAsync(string campaignId) {
			try {
				if (campaignId.StartsWith("cmp_")) {
					campaignId = campaignId.Substring(4);
				}

				var queryParams = new Dictionary<string, string>
				{
					{ "limit", "0" }
				};

				return await _apiClient.GetAsync($"agent-hub/campaigns/{campaignId}", queryParams);
			} catch (Exception ex) {
				throw new Exception("Error retrieving campaign details.", ex);
			}
		}

		/// Update an existing campaign.
		public async Task<JObject> UpdateCampaignAsync(string campaignId, JObject updatedData) {
			try {
				if (campaignId.StartsWith("cmp_")) {
					campaignId = campaignId.Substring(4);
				}

				return await _apiClient.PatchAsync($"agent-hub/campaigns/{campaignId}", updatedData);
			} catch (Exception ex) {
				throw new Exception("Error updating campaign.", ex);
			}
		}

		/// Retrieve all accounts.
		public async Task<JObject> GetAllAccountsAsync() {
			try {
				return await _apiClient.GetAsync("account/my-accounts");
			} catch (Exception ex) {
				throw new Exception("Error retrieving accounts.", ex);
			}
		}
	}
}
