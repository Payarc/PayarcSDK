using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

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

			var responseDetails = new JObject {
				["IsSuccess"] = response.IsSuccessStatusCode,
				["StatusCode"] = (int)response.StatusCode,
				["ReasonPhrase"] = response.ReasonPhrase,
				["Content"] = content
			};

			if (!response.IsSuccessStatusCode) {
				return responseDetails; // Return the structured error response.
			}

			try {
				var jsonContent = JObject.Parse(content);
				responseDetails["Data"] = jsonContent;
				return responseDetails;
			} catch (Exception ex) {
				throw new Exception("Failed to parse response content to JSON.", ex);
			}
		}
	}
}
