using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace PayarcSDK.Entities.PayarcConnectService {
    public class TerminalResponse {
        [JsonProperty("Terminals")]
        public List<Terminal> Terminals { get; set; } = new List<Terminal>();

        [JsonProperty("ErrorCode")]
        public int ErrorCode { get; set; }

        [JsonProperty("ErrorMessage")]
        public string? ErrorMessage { get; set; }
    }

    public class Terminal {
        [JsonProperty("Object")]
        public string? Object { get; set; }

        [JsonProperty("Id")]
        public string? Id { get; set; }

        [JsonProperty("Terminal")]
        public string? TerminalName { get; set; }

        [JsonProperty("Type")]
        public string? Type { get; set; }

        [JsonProperty("Code")]
        public string? Code { get; set; }

        [JsonProperty("Is_enabled")]
        public bool? IsEnabled { get; set; }

        [JsonProperty("Device_id")]
        public string? DeviceId { get; set; }

        [JsonProperty("Pos_identifier")]
        public string? PosIdentifier { get; set; }

        [JsonProperty("Created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("Updated_at")]
        public DateTime UpdatedAt { get; set; }
    }
}
