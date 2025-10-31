using PayarcSDK.Entities;
using PayarcSDK.Entities.UserSetting;
using System.Text;
using System.Text.Json;

namespace PayarcSDK.Services {
    public class UserSettingService : CommonServices {
        private readonly HttpClient _httpClient;

        public UserSettingService(HttpClient httpClient) : base(httpClient) {
            _httpClient = httpClient;
        }

        public async Task<BaseResponse> CreateOrUpdate(UserSettingRequestData settingData) {
            return await CreateOrUpdateUserSettingAsync(settingData);
        }

        public async Task<ListBaseResponse> List(BaseListOptions? options = null) {
            return await GetAllUserSettingsAsync(options);
        }

        public async Task<bool> Delete(string settingKey) {
            return await DeleteUserSettingAsync(settingKey);
        }

        /// <summary>
        /// Create or update a user setting.
        /// </summary>
        private async Task<BaseResponse> CreateOrUpdateUserSettingAsync(UserSettingRequestData settingData) {
            try {
                var url = "my-user-settings";
                var content = new StringContent(settingData.ToJson(), Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(url, content);
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrWhiteSpace(responseContent))
                    throw new InvalidOperationException("Response body is empty.");

                var data = JsonSerializer.Deserialize<Dictionary<string, object>>(responseContent);
                if (data != null && data.TryGetValue("data", out var dataValue) && dataValue is JsonElement dataElement) {
                    var dataDict = JsonSerializer.Deserialize<Dictionary<string, object>>(dataElement.GetRawText());
                    if (dataDict != null) {
                        var result = TransformJsonRawObject(dataDict, dataElement.GetRawText(), "UserSetting");
                        return result ?? throw new InvalidOperationException("Failed to transform user setting data.");
                    }
                }

                throw new InvalidOperationException("Response data is invalid or missing.");
            } catch (HttpRequestException ex) {
                Console.WriteLine($"HTTP error processing user setting: {ex.Message}");
                throw;
            } catch (JsonException ex) {
                Console.WriteLine($"JSON error processing user setting: {ex.Message}");
                throw new InvalidOperationException("Failed to process JSON response.", ex);
            } catch (Exception ex) {
                Console.WriteLine($"General error handling user setting: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Get all user settings with optional search parameters.
        /// </summary>
        private async Task<ListBaseResponse> GetAllUserSettingsAsync(BaseListOptions? options = null) {
            try {
                var parameters = new Dictionary<string, object?> {
                    { "limit", options?.Limit ?? 99999 },
                    { "page", options?.Page ?? 1 }
                };

                if (options?.Constraint != null) {
                    foreach (var kvp in options.Constraint) {
                        parameters[kvp.Key] = kvp.Value;
                    }
                }

                var query = BuildQueryString(parameters);
                var url = "my-user-settings";
                if (!string.IsNullOrWhiteSpace(query))
                    url = $"{url}?{query}";

                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrWhiteSpace(responseContent))
                    throw new InvalidOperationException("Response body is empty.");

                var responseData = JsonSerializer.Deserialize<Dictionary<string, object>>(responseContent);
                if (responseData == null || !responseData.TryGetValue("data", out var dataValue) || !(dataValue is JsonElement dataElement)) {
                    throw new InvalidOperationException("Response data is invalid or missing.");
                }

                var rawData = dataElement.GetRawText();
                var jsonSettings = dataElement.Deserialize<List<Dictionary<string, object>>>();
                List<BaseResponse?>? userSettings = new List<BaseResponse?>();

                if (jsonSettings != null) {
                    foreach (var setting in jsonSettings) {
                        var item = TransformJsonRawObject(setting, JsonSerializer.Serialize(setting), "UserSetting");
                        userSettings?.Add(item);
                    }
                }

                return new UserSettingListResponse {
                    Data = userSettings,
                    RawData = rawData
                };
            } catch (HttpRequestException ex) {
                Console.WriteLine($"HTTP error retrieving user settings: {ex.Message}");
                throw;
            } catch (JsonException ex) {
                Console.WriteLine($"JSON error retrieving user settings: {ex.Message}");
                throw new InvalidOperationException("Failed to process JSON response.", ex);
            } catch (Exception ex) {
                Console.WriteLine($"General error retrieving user settings: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Delete a user setting by key.
        /// </summary>
        private async Task<bool> DeleteUserSettingAsync(string settingKey) {
            try {
                var url = "my-user-settings";
                var requestData = new { key = settingKey };
                var content = new StringContent(JsonSerializer.Serialize(requestData), Encoding.UTF8, "application/json");

                var request = new HttpRequestMessage(HttpMethod.Delete, url) { Content = content };
                var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                Console.WriteLine($"Successfully deleted user setting: {settingKey}");
                return true;
            } catch (HttpRequestException ex) {
                Console.WriteLine($"HTTP error deleting user setting: {ex.Message}");
                throw;
            } catch (Exception ex) {
                Console.WriteLine($"General error deleting user setting: {ex.Message}");
                return false;
            }
        }
    }
}