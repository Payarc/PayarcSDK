using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace PayarcSDK.Http {
	public class ApiClient {
		private readonly HttpClient _httpClient;

		public ApiClient(HttpClient httpClient) {
			_httpClient = httpClient;
		}

		public async Task<JObject> GetAsync(string endpoint, Dictionary<string, string> queryParams = null) {
			if (queryParams != null && queryParams.Count > 0) {
				var queryString = string.Join("&", queryParams.Select(kvp => $"{kvp.Key}={Uri.EscapeDataString(kvp.Value)}"));
				endpoint = $"{endpoint}?{queryString}";
			}

			var response = await _httpClient.GetAsync(endpoint);
			return await ProcessResponse(response);
		}

		public async Task<JObject> PostAsync(string endpoint, JObject request) {
			var content = new StringContent(request.ToString(), Encoding.UTF8, "application/json");
			var response = await _httpClient.PostAsync(endpoint, content);
			return await ProcessResponse(response);
		}

		public async Task<JObject> PostRawAsync(string endpoint, HttpContent content) {
			var requestMessage = new HttpRequestMessage(HttpMethod.Post, endpoint) {
				Content = content
			};

			var response = await _httpClient.SendAsync(requestMessage);
			return await ProcessResponse(response);
		}

		public async Task<JObject> PatchAsync(string endpoint, JObject request) {
			var content = new StringContent(request.ToString(), Encoding.UTF8, "application/json");
			var requestMessage = new HttpRequestMessage(new HttpMethod("PATCH"), endpoint) {
				Content = content
			};

			var response = await _httpClient.SendAsync(requestMessage);
			return await ProcessResponse(response);
		}

		public async Task DeleteAsync(string endpoint) {
			var response = await _httpClient.DeleteAsync(endpoint);
			await ProcessResponse(response);
		}

		private async Task<JObject> ProcessResponse(HttpResponseMessage response) {
			string content = await response.Content.ReadAsStringAsync();
			// Initialize the response details
			var responseDetails = new JObject {
				["IsSuccess"] = response.IsSuccessStatusCode,
				["StatusCode"] = (int)response.StatusCode,
				["ReasonPhrase"] = response.ReasonPhrase,
				["Content"] = content
			};

			if (!response.IsSuccessStatusCode) {
				return responseDetails;
			}

			if (string.IsNullOrWhiteSpace(content)) {
				responseDetails["Data"] = null;
				return responseDetails;
			}

			try {
				var jsonContent = JToken.Parse(content);

				if (jsonContent.Type == JTokenType.Object) {
					responseDetails["Data"] = (JObject)jsonContent;
				} else {
					responseDetails["Data"] = new JArray(jsonContent);
				}

				return responseDetails;
			} catch (JsonReaderException ex) {
				throw new Exception("Failed to parse response content to JSON. Raw content: " + content, ex);
			}
		}
	}
}