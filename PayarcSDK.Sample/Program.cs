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
			var bankData = new List<JObject>();
			var customerId = "";

			// Add a card to a customer
			cardData.Add(new JObject {
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
			});

			// Add a card to a customer
			cardData.Add(new JObject {
				["card_source"] = "INTERNET",
				["card_number"] = "4111111111111111",
				["exp_month"] = "02",
				["exp_year"] = "2027",
				["cvv"] = "999",
				["card_holder_name"] = "John Doe",
				["address_line1"] = "411 West Putnam Avenue",
				["city"] = "New York",
				["state"] = "NY",
				["zip"] = "06830",
				["country"] = "US"

			});

			// Add a bank info to a customer
			bankData.Add(new JObject {
				["account_number"] = 1234567890,
				["routing_number"] = 123456789,
				["first_name"] = "Test",
				["last_name"] = "Account",
				["account_type"] = "Personal Checking",
				["sec_code"] = "TEL"
			});

			// Add a bank info to a customer
			bankData.Add(new JObject {
				["account_number"] = 1234567890,
				["routing_number"] = 123456789,
				["first_name"] = "Test2",
				["last_name"] = "Account2",
				["account_type"] = "Personal Checking",
				["sec_code"] = "TEL"
			});

			// List customers
			var queryParams = new Dictionary<string, string> {
				["limit"] = "10",
				["page"] = "1"
			};

			var customers = await customerService.list(queryParams);
			Console.WriteLine($"List of Customers: {customers}");

			customerId = customers["data"]
				.Children<JObject>()
				.Where(p => (string)p.SelectToken("name") == "Shah Test5")
				.Select(p => (string)p.SelectToken("customer_id")).FirstOrDefault();

			var testPurpose = "deleteCustomer";
			switch (testPurpose) {
				case "createCustomer":
					// Create a new customer
					var newCustomerData = new JObject {
						["name"] = "Shah Test8",
						["email"] = "shah@test8.com",
						["phone"] = "1234567890"
					};
					newCustomerData.Add("cards", JToken.FromObject(cardData));
					newCustomerData.Add("bank_accounts", JToken.FromObject(bankData));
					var createdCustomer = await customerService.create(newCustomerData);
					Console.WriteLine($"Created Customer: {createdCustomer}");
					// Retrieve a customer
					customerId = (string)createdCustomer.SelectToken("customer_id");
					var customer = await customerService.retrieve(customerId);
					Console.WriteLine($"Retrieved Customer: {customer}");
					break;
				case "updateCustomer":
					// Update a customer
					var customerData = new JObject {
						["description"] = "Example customer add card",
						["email"] = "shahupdate2@sdk.com",
						["phone"] = "2222222222"
					};
					customerData.Add("cards", JToken.FromObject(cardData));
					customerData.Add("bank_accounts", JToken.FromObject(bankData));
					var updatedCustomer = await customerService.update(customerId, customerData);
					Console.WriteLine($"Updated Customer: {updatedCustomer}");
					break;
				case "deleteCustomer":
					// Delete a customer
					var deleted = await customerService.delete(customerId);
					Console.WriteLine($"Deleted Customer: {deleted}");
					break;
				default:
					Console.WriteLine("Nothing to test.");
					break;
			}
		}
	}
}
