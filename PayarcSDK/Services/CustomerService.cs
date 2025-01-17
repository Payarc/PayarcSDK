using Newtonsoft.Json.Linq;
using PayarcSDK.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PayarcSDK.Services {
	public class CustomerService {
		private readonly ApiClient _apiClient;

		public CustomerService(ApiClient apiClient) {
			_apiClient = apiClient;
		}

		public async Task<JObject> CreateCustomerAsync(JObject customerData, List<JObject> cardDataList) {
			if (cardDataList != null) {
				// Check count of cardDataList if creater then 0 then do code below.
				foreach (JObject cardData in cardDataList) {
					var cardToken = await _apiClient.PostAsync("tokens", cardData);
					var tokenId = cardToken["data"]["id"]?.ToString();
					customerData["token_id"] = tokenId;
				}
			}
			return await _apiClient.PostAsync("customers", customerData);
		}

		public async Task<JObject> RetrieveCustomerAsync(string customerId) {
			return await _apiClient.GetAsync($"customers/{customerId}");
		}

		public async Task<JObject> UpdateCustomerAsync(string customerId, JObject customerData) {
			return await _apiClient.PatchAsync($"customers/{customerId}", customerData);
		}

		public async Task<JObject> AddCardToCustomerAsync(string customerId, JObject cardData) {
			var cardToken = await _apiClient.PostAsync("tokens", cardData);
			var tokenId = cardToken["data"]["id"]?.ToString();
			var updateData = new JObject { ["token_id"] = tokenId };
			return await _apiClient.PatchAsync($"customers/{customerId}", updateData);
		}

		public async Task<JObject> AddBankAccountToCustomerAsync(string customerId, JObject bankData) {
			bankData["customer_id"] = customerId;
			return await _apiClient.PostAsync("bankaccounts", bankData);
		}

		public async Task<JObject> ListCustomersAsync(Dictionary<string, string> queryParams = null) {
			return await _apiClient.GetAsync("customers", queryParams);
		}
	}
}