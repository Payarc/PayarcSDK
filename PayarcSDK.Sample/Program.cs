using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PayarcSDK;
using PayarcSDK.Configuration;
using PayarcSDK.Http;
using PayarcSDK.Models;
using PayarcSDK.Models.Responses;
using PayarcSDK.Services;
using System.Net.Http;
using System.Text;

namespace PayarcSDK.Sample {

	internal class Program {
		static async Task Main(string[] args) {
			var requestBody = new {
				username = "shahsuvar@payarc.com",
				password = "6qCUnwTD.4K_CVz"
			};

			var requestContent = new StringContent(
			JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
			var httpClient = new HttpClient();
			var response = await httpClient.PostAsync("https://testapi.payarc.net/v1/login", requestContent);

			if (!response.IsSuccessStatusCode) {
				var errorContent = await response.Content.ReadAsStringAsync();
				throw new InvalidOperationException($"Failed to retrieve access token. StatusCode: {response.StatusCode}, Content: {errorContent}");
			}

			// Deserialize the success response
			var responseContent = await response.Content.ReadAsStringAsync();
			var successResponse = JsonConvert.DeserializeObject<AddSuccessResponse>(responseContent);

			// Retrieve the access token
			var accessToken = successResponse?.response_content?.access_token;

			if (string.IsNullOrEmpty(accessToken)) {
				throw new InvalidOperationException("Access token is missing in the response.");
			}

			var client = new SdkBuilder()
				.Configure(config => {
					config.Environment = "sandbox";     // Use sandbox environment
					config.ApiVersion = "v1";           // Use version 2 of the API
					config.BearerToken = accessToken;   // Set the Bearer Token
				})
				.Build();

			// Initialize the service
			var customerService = new CustomerService(client);
			var cardData = new List<JObject>();
			var customerId = "";

			if (false) {
				// Create a new customer
				var newCustomerData = new JObject {
					["name"] = "Shah Test3",
					["email"] = "shah@test3.com",
					["phone"] = "1234567890"
				};

				var createdCustomer = await customerService.CreateCustomerAsync(newCustomerData, cardData);
				Console.WriteLine($"Created Customer: {createdCustomer}");

				// Retrieve a customer
				//var customerId = "cus_12345";
				customerId = (string)createdCustomer.SelectToken("customer_id");
				var customer = await customerService.RetrieveCustomerAsync(customerId);
				Console.WriteLine($"Retrieved Customer: {customer}");
			} else {

				// List customers
				var queryParams = new Dictionary<string, string> {
					["limit"] = "10",
					["page"] = "1"
				};

				var customers = await customerService.ListCustomersAsync(queryParams);
				Console.WriteLine($"List of Customers: {customers}");

				customerId = customers["data"]
					.Children<JObject>()
					.Where(p => (string)p.SelectToken("name") == "Shah Update2")
					.Select(p => (string)p.SelectToken("customer_id")).FirstOrDefault();
			}

			// Update a customer
			var updateData = new JObject {
				["description"] = "Example customer add card",
				["email"] = "shahtest@sdk.com",
				["phone"] = "1231231234"
			};

			var updatedCustomer = await customerService.UpdateCustomerAsync(customerId, updateData);
			Console.WriteLine($"Updated Customer: {updatedCustomer}");

			// Add a card to a customer
			cardData.Add(
				new JObject {
					["card_source"] = "INTERNET",
					["card_number"] = "4012000098765439",
					["exp_month"] = "12",
					["exp_year"] = "2025",
					["cvv"] = "999",
					["card_holder_name"] = "John Doe",
					["address_line1"] = "411 West Putnam Avenue",
					["city"] = "Greenwich",
					["state"] = "CT",
					["zip"] = "06840",
					["country"] = "US"
				}

				);
			//update logic for this cardData into List of JObject.
			//var updatedWithCard = await customerService.AddCardToCustomerAsync(customerId, cardData);
			//Console.WriteLine($"Customer with Added Card: {updatedWithCard}");
		}
	}
}
