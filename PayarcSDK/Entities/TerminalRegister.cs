namespace PayarcSDK.Entities;

using Newtonsoft.Json;

public class TerminalRegister
{
    [JsonProperty("terminal")]
    public string? Terminal { get; set; }

    [JsonProperty("type")]
    public string? Type { get; set; }

    [JsonProperty("code")]
    public string? Code { get; set; }

    [JsonProperty("is_enabled")]
    public bool? IsEnabled { get; set; }

    [JsonProperty("device_id")]
    public string? DeviceId { get; set; }

    [JsonProperty("pos_identifier")]
    public string? PosIdentifier { get; set; }

    [JsonProperty("datawire_client_id")]
    public string? DatawireClientId { get; set; }
}