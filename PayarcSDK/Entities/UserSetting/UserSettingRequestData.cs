using System.Text.Json.Serialization;

namespace PayarcSDK.Entities.UserSetting {
    public class UserSettingRequestData {
        [JsonPropertyName("key")]
        public string Key { get; set; } = string.Empty;

        [JsonPropertyName("value")]
        public string Value { get; set; } = string.Empty;

        public string ToJson() => System.Text.Json.JsonSerializer.Serialize(this);
    }
}