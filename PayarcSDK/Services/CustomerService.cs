using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PayarcSDK.Entities;
using PayarcSDK.Models;
using JsonException = System.Text.Json.JsonException;
using JsonSerializer = System.Text.Json.JsonSerializer;
using PayarcSDK.Services;
using AnyOfTypes;

namespace PayarcSDK.Services {
	public class CustomerService {
		private readonly HttpClient _httpClient;

		public CustomerService(HttpClient httpClient) {
			_httpClient = httpClient;
		}

		public async Task<CustomerResponseData> Create(CustomerInfoData customerData) {
			return await CreateCustomerAsync(customerData);
		}

		public async Task<CustomerResponseData> Retrieve(string customerId) {
			return await RetrieveCustomerAsync(customerId);
		}

		public async Task<ListBaseResponse> List(OptionsData options) {
			return await ListCustomersAsync(options);
		}

		public async Task<JObject> Update(AnyOf<string?, CustomerResponseData> customer,
			CustomerInfoData? customerData = null) {
			string? customerId = string.Empty;
			customerId = customer.IsSecond ? customer.Second.ObjectId : customer.First;
			return await UpdateCustomerAsync(customerId, customerData);
		}

		public async Task<bool> Delete(string customerId) {
			return await DeleteCustomerAsync(customerId);
		}

		private async Task<CustomerResponseData> CreateCustomerAsync(CustomerInfoData customerData) {
			var response = await CreateCustomer("customers", customerData);
			CustomerResponseData createdCustomer = response;
			var customerId = createdCustomer.CustomerId?.ToString();

			if (customerData.Cards.Count() != 0) {
				foreach (CardData cardData in customerData.Cards) {
					await AddCardToCustomerAsync(customerId, cardData, customerData);
				}
			}

			if (customerData.BankAccounts.Count() != 0) {
				foreach (BankData bankData in customerData.BankAccounts) {
					await AddBankAccountToCustomerAsync(customerId, bankData);
				}
			}

			createdCustomer = await RetrieveCustomerAsync(customerId);
			return createdCustomer;
		}

		private async Task<CustomerResponseData> RetrieveCustomerAsync(string customerId) {
			var url = $"customers/{customerId}";
			var response = await _httpClient.GetAsync(url);
			response.EnsureSuccessStatusCode();

			var responseContent = await response.Content.ReadAsStringAsync();
			return JsonConvert.DeserializeObject<CustomerResponseData>(responseContent);
		}

		private async Task<JObject> UpdateCustomerAsync(string customerId, CustomerInfoData customerData) {
			if (customerData.Cards.Count() != 0) {
				foreach (CardData cardData in customerData.Cards) {
					await AddCardToCustomerAsync(customerId, cardData, customerData);
				}
			}

			if (customerData.BankAccounts.Count() != 0) {
				foreach (BankData bankData in customerData.BankAccounts) {
					await AddBankAccountToCustomerAsync(customerId, bankData);
				}
			}

			return await UpdateCustomer($"customers/{customerId}", customerData);
		}

		private async Task<JObject> AddCardToCustomerAsync(string customerId, CardData cardData, CustomerInfoData customerData) {
			var cardToken = await CreateCardToken("tokens", cardData);
			var tokenId = cardToken["data"]?["id"]?.ToString();
			customerData.TokenId = tokenId;
			return await UpdateCustomer($"customers/{customerId}", customerData);
		}

		private async Task<JObject> AddBankAccountToCustomerAsync(string customerId, BankData bankData) {
			bankData.CustomerId = customerId;
			return await AddBankAccount("bankaccounts", bankData);
		}

		private async Task<ListBaseResponse> ListCustomersAsync(OptionsData options) {
			try {
				var parameters = new Dictionary<string, object>
				{
					{ "limit", options.Limit ?? 25 },
					{ "page", options.Page ?? 1 }
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
				CommonServices commonServices = new CommonServices();
				var query = commonServices.BuildQueryString(parameters);
				return await GetCustomersAsync("customers", query);
			} catch (Exception ex) {
				Console.WriteLine(ex);
				throw;
			}
		}

		private async Task<bool> DeleteCustomerAsync(string customerId) {
			await DeleteAsync($"customers/{customerId}");
			return true;
		}

		// Generic HTTP request methods
		private async Task<CustomerResponseData> CreateCustomer(string url, CustomerInfoData customerData) {
			//var jsonData = JsonConvert.SerializeObject(data);
			Console.WriteLine(customerData.ToJson());
			var content = new StringContent(customerData.ToJson(), Encoding.UTF8, "application/json");
			var response = await _httpClient.PostAsync(url, content);
			response.EnsureSuccessStatusCode();
			var responseContent = await response.Content.ReadAsStringAsync();
			return JsonConvert.DeserializeObject<CustomerResponseData>(responseContent);
		}

		private async Task<JObject> UpdateCustomer(string url, CustomerInfoData customerData) {
			var content = new StringContent(customerData.ToJson(), Encoding.UTF8, "application/json");
			var request = new HttpRequestMessage(HttpMethod.Patch, url) { Content = content };
			var response = await _httpClient.SendAsync(request);
			response.EnsureSuccessStatusCode();
			var responseContent = await response.Content.ReadAsStringAsync();
			return JObject.Parse(responseContent);
		}

		private async Task<JObject> CreateCardToken(string url, CardData cardData) {
			var content = new StringContent(cardData.ToJson(), Encoding.UTF8, "application/json");
			var response = await _httpClient.PostAsync(url, content);
			response.EnsureSuccessStatusCode();
			var responseContent = await response.Content.ReadAsStringAsync();
			return JObject.Parse(responseContent);
		}

		private async Task<JObject> AddBankAccount(string url, BankData bankData) {
			var content = new StringContent(bankData.ToJson(), Encoding.UTF8, "application/json");
			var response = await _httpClient.PostAsync(url, content);
			response.EnsureSuccessStatusCode();
			var responseContent = await response.Content.ReadAsStringAsync();
			return JObject.Parse(responseContent);
		}

		private async Task<ListBaseResponse> GetCustomersAsync(string url, string? queryParams) {
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
				var jsonCustomers = dataElement.Deserialize<List<Dictionary<string, object>>>();
				List<BaseResponse?>? customers = new List<BaseResponse?>();
				if (jsonCustomers != null) {
					for (int i = 0; i < jsonCustomers.Count; i++) {
						var customer = AddObjectId(jsonCustomers[i], JsonSerializer.Serialize(jsonCustomers[i]));
						customers?.Add(customer);
					}
				}

				var pagination = new Dictionary<string, object>();
				if (responseData.TryGetValue("meta", out var metaValue) && metaValue is JsonElement metaElement) {
					var paginationElement = metaElement.GetProperty("pagination");
					pagination["total"] = paginationElement.GetProperty("total").GetInt32();
					pagination["count"] = paginationElement.GetProperty("count").GetInt32();
					pagination["per_page"] = paginationElement.GetProperty("per_page").GetInt32();
					pagination["current_page"] = paginationElement.GetProperty("current_page").GetInt32();
					pagination["total_pages"] = paginationElement.GetProperty("total_pages").GetInt32();
				}

				pagination?.Remove("links");

				return new CustomerListResponse {
					Data = customers,
					Pagination = pagination,
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

		private async Task DeleteAsync(string url) {
			var response = await _httpClient.DeleteAsync(url);
			response.EnsureSuccessStatusCode();
		}

		private BaseResponse? AddObjectId(Dictionary<string, object> obj, string? rawObj) {
			BaseResponse? response = null;
			if (obj["object"]?.ToString() == "customer") {
				if (rawObj != null) {
					var customerResponse = JsonConvert.DeserializeObject<CustomerResponseData>(rawObj);
					if (customerResponse == null) {
						customerResponse = new CustomerResponseData();
					}

					customerResponse.RawData = rawObj;
					customerResponse.ObjectId ??= $"cus_{obj["customer_id"]}";
					customerResponse.Update = async (customerData) => {
						var result = await Update(customerResponse, customerData);
						return JsonConvert.DeserializeObject<CustomerResponseData>(result.ToString());
					};
					response = customerResponse;
				}
			}

			return response;
		}
	}
}
