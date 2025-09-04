using AnyOfTypes;
using Newtonsoft.Json;
using PayarcSDK.Entities;
using PayarcSDK.Entities.Batch;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Web;
using JsonException = System.Text.Json.JsonException;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace PayarcSDK.Services
{
    public class DepositService : CommonServices
    {
        private readonly HttpClient _httpClient;

		public DepositService(HttpClient httpClient) : base(httpClient) {
			_httpClient = httpClient;
		}

		public async Task<ListBaseResponse?> List(BaseListOptions? options = null) {
			try {
				var parameters = new Dictionary<string, object?>
					{
						{ "from_date", options?.From_Date },
						{ "to_date", options?.To_Date }
					}.Where(kvp => kvp.Value != null)
					.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

				var query = BuildQueryString(parameters);
				return await AgentDepositSummary("agent/deposit/summary", query);
			} catch (Exception ex) {
				Console.WriteLine(ex);
				throw;
			}
		}

		private async Task<ListBaseResponse?> AgentDepositSummary(string endpoint, string? queryParams, string type = "Account") {
			try {
				var url = $"{endpoint}?{queryParams}";
				var request = new HttpRequestMessage(HttpMethod.Get, url);
				var response = await _httpClient.SendAsync(request);
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
				var jsonAccounts = dataElement.Deserialize<List<Dictionary<string, object>>>();
				List<BaseResponse?>? accounts = new List<BaseResponse?>();
				if (jsonAccounts != null) {
					for (int i = 0; i < jsonAccounts.Count; i++) {
						var ch = TransformJsonRawObject(jsonAccounts[i], JsonSerializer.Serialize(jsonAccounts[i]), type);
						accounts?.Add(ch);
					}
				}
				return new DepositListResponse {
					Data = accounts,
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
	}
}