using Newtonsoft.Json.Linq;
using System.Text;

namespace PayarcSDK.Services {
	public class SplitCampaignService {
		private readonly HttpClient _httpClient;

		public SplitCampaignService(HttpClient httpClient) {
			_httpClient = httpClient;
		}

		public async Task<JObject> create(JObject campaignData) {
			return await CreateCampaignAsync(campaignData);
		}

		public async Task<JObject> list(Dictionary<string, string> queryParams = null) {
			return await GetAllCampaignsAsync(queryParams);
		}

		public async Task<JObject> retrieve(string campaignId) {
			return await GetCampaignDetailsAsync(campaignId);
		}

		public async Task<JObject> update(string campaignId, JObject updatedData) {
			return await UpdateCampaignAsync(campaignId, updatedData);
		}

		public async Task<JObject> listAccounts() {
			return await GetAllAccountsAsync();
		}

		/// Create a new campaign.
		private async Task<JObject> CreateCampaignAsync(JObject campaignData) {
			try {
				return await PostAsync("agent-hub/campaigns", campaignData);
			} catch (Exception ex) {
				throw new Exception("Error creating campaign.", ex);
			}
		}

		/// Retrieve all campaigns.
		private async Task<JObject> GetAllCampaignsAsync(Dictionary<string, string> queryParams = null) {
			try {
				return await GetAsync("agent-hub/campaigns", queryParams);
			} catch (Exception ex) {
				throw new Exception("Error retrieving campaigns.", ex);
			}
		}

		/// Retrieve details of a specific campaign by ID.
		private async Task<JObject> GetCampaignDetailsAsync(string campaignId) {
			try {
				if (campaignId.StartsWith("cmp_")) {
					campaignId = campaignId.Substring(4);
				}

				var queryParams = new Dictionary<string, string>
				{
					{ "limit", "0" }
				};

				return await GetAsync($"agent-hub/campaigns/{campaignId}", queryParams);
			} catch (Exception ex) {
				throw new Exception("Error retrieving campaign details.", ex);
			}
		}

		/// Update an existing campaign.
		private async Task<JObject> UpdateCampaignAsync(string campaignId, JObject updatedData) {
			try {
				if (campaignId.StartsWith("cmp_")) {
					campaignId = campaignId.Substring(4);
				}

				return await PatchAsync($"agent-hub/campaigns/{campaignId}", updatedData);
			} catch (Exception ex) {
				throw new Exception("Error updating campaign.", ex);
			}
		}

		/// Retrieve all accounts.
		private async Task<JObject> GetAllAccountsAsync() {
			try {
				return await GetAsync("account/my-accounts");
			} catch (Exception ex) {
				throw new Exception("Error retrieving accounts.", ex);
			}
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
