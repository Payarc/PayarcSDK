﻿using Newtonsoft.Json.Linq;
using PayarcSDK.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PayarcSDK.Services {
	public class CustomerService {
		private readonly ApiClient _apiClient;

		public CustomerService(ApiClient apiClient) {
			_apiClient = apiClient;
		}

		public async Task<JObject> create(JObject customerData) { 
			return await CreateCustomerAsync(customerData);
		}
		public async Task<JObject> retrieve(string customerId) {
			return await RetrieveCustomerAsync(customerId);
		}
		public async Task<JObject> list(Dictionary<string, string> queryParams = null) {
			return await ListCustomersAsync(queryParams);
		}
		public async Task<JObject> update(string customerId, JObject customerData) {
			return await UpdateCustomerAsync(customerId, customerData);
		}

		private async Task<JObject> CreateCustomerAsync(JObject customerData) {
			JObject createdCustomer = new JObject();
			if (customerData["cards"] != null) {
				createdCustomer = await _apiClient.PostAsync("customers", customerData);
				var customerId = createdCustomer["data"]["customer_id"]?.ToString();
				foreach (JObject cardData in customerData["cards"]) {
					await AddCardToCustomerAsync(customerId, cardData);
				}
				createdCustomer = await RetrieveCustomerAsync(customerId);
			} else { 
				createdCustomer = await _apiClient.PostAsync("customers", customerData);
			}
			return createdCustomer;
		}

		private async Task<JObject> RetrieveCustomerAsync(string customerId) {
			return await _apiClient.GetAsync($"customers/{customerId}");
		}

		private async Task<JObject> UpdateCustomerAsync(string customerId, JObject customerData) {
			JObject updatedCustomer = new JObject();
			if (customerData["cards"] != null) {
				foreach (JObject cardData in customerData["cards"]) {
					await AddCardToCustomerAsync(customerId, cardData);
				}
				updatedCustomer = await _apiClient.PatchAsync($"customers/{customerId}", customerData);
			} else {
				updatedCustomer = await _apiClient.PatchAsync($"customers/{customerId}", customerData);
			}
			return updatedCustomer;
		}

		private async Task<JObject> AddCardToCustomerAsync(string customerId, JObject cardData) {
			var cardToken = await _apiClient.PostAsync("tokens", cardData);
			var tokenId = cardToken["data"]["id"]?.ToString();
			var updateData = new JObject { ["token_id"] = tokenId };
			return await _apiClient.PatchAsync($"customers/{customerId}", updateData);
		}

		private async Task<JObject> AddBankAccountToCustomerAsync(string customerId, JObject bankData) {
			bankData["customer_id"] = customerId;
			return await _apiClient.PostAsync("bankaccounts", bankData);
		}

		private async Task<JObject> ListCustomersAsync(Dictionary<string, string> queryParams = null) {
			return await _apiClient.GetAsync("customers", queryParams);
		}
	}
}