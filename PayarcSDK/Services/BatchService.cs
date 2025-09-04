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
    public class BatchService : CommonServices
    {
        private readonly HttpClient _httpClient;

		public BatchService(HttpClient httpClient) : base(httpClient) {
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
				return await ListBatchReportsByAgent("agent/batch/reports", query);
			} catch (Exception ex) {
				Console.WriteLine(ex);
				throw;
			}
		}

		public async Task<ListBaseResponse?> Retrieve(BaseListOptions? options = null) {
			try {
				var parameters = new Dictionary<string, object?>
					{
						{ "reference_number", options?.Reference_Number },
						{ "date", options?.Date }
					}.Where(kvp => kvp.Value != null)
					.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

				var query = BuildQueryString(parameters);
				return await ListBatchReportDetailsByAgent("agent/batch/reports/details/" + options?.Merchant_Account_Number, query);
			} catch (Exception ex) {
				Console.WriteLine(ex);
				throw;
			}
		}

		private async Task<ListBaseResponse?> ListBatchReportsByAgent(string endpoint, string? queryParams, string type = "Batch") {
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
				var jsonBatches = dataElement.Deserialize<List<Dictionary<string, object>>>();
				List<BaseResponse?>? batches = new List<BaseResponse?>();
				if (jsonBatches != null) {
					for (int i = 0; i < jsonBatches.Count; i++) {
						var ch = TransformJsonRawObject(jsonBatches[i], JsonSerializer.Serialize(jsonBatches[i]), type);
						batches?.Add(ch);
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

				return new BatchListResponse {
					Data = batches,
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

		private async Task<BatchDetailResponse?> ListBatchReportDetailsByAgent(string endpoint, string? queryParams, string type = "BatchDetail") {
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
				if (string.IsNullOrWhiteSpace(responseBody))
					throw new InvalidOperationException("Response body is empty.");
				using var doc = JsonDocument.Parse(responseBody);
				var root = doc.RootElement.Clone();
				var responseData = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(root.GetProperty("data").GetRawText());
				if (responseData == null)
					throw new InvalidOperationException("Response data is invalid or missing.");
				var updatedData = new Dictionary<string, object>();
				foreach (var kvp in responseData) {
					if (kvp.Key == "grand_total") {
						updatedData[kvp.Key] = kvp.Value.Deserialize<GrandTotal>();
						continue;
					}
					var batchDetails = kvp.Value.Deserialize<BatchDetails>();
					var transformedList = new List<BatchData>();
					if (kvp.Value.TryGetProperty("batch_data", out var batchDataEl) &&
						batchDataEl.ValueKind == JsonValueKind.Array) {
						foreach (var itemEl in batchDataEl.EnumerateArray()) {
							// Deserialize each batch_data element to Dictionary
							var rawDict = itemEl.Deserialize<Dictionary<string, object>>(
								new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
							);

							var rawJson = itemEl.GetRawText();
							var transformed = TransformJsonRawObject(rawDict!, rawJson, type) as BatchData;
							if (transformed != null)
								transformedList.Add(transformed);
						}
					}
					updatedData[kvp.Key] = new {
						batch_data = transformedList,
						batch_total = batchDetails?.BatchTotal
					};
				}
				var batchListResponse = new BatchDetailResponse {
					Data = new List<BaseResponse?> { new BatchWrapper { Data = updatedData } },
					Pagination = new Dictionary<string, object>(),
					RawData = root.GetProperty("data").GetRawText()
				};
				if (responseData.TryGetValue("meta", out var metaValue) && metaValue.ValueKind == JsonValueKind.Object) {
					var metaElement = metaValue;
					var paginationElement = metaElement.GetProperty("pagination");
					batchListResponse.Pagination["total"] = paginationElement.GetProperty("total").GetInt32();
					batchListResponse.Pagination["count"] = paginationElement.GetProperty("count").GetInt32();
					batchListResponse.Pagination["per_page"] = paginationElement.GetProperty("per_page").GetInt32();
					batchListResponse.Pagination["current_page"] = paginationElement.GetProperty("current_page").GetInt32();
					batchListResponse.Pagination["total_pages"] = paginationElement.GetProperty("total_pages").GetInt32();
					batchListResponse.Pagination.Remove("links");
				}
				return batchListResponse;
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

		public class BatchWrapper : BaseResponse {
			public Dictionary<string, object> Data { get; set; } = new();
		}
	}
}