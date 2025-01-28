using Newtonsoft.Json;

namespace PayarcSDK.Entities;

public class UpdatePlanOptions
{
    [JsonProperty("trial_period_days")]
    public int? TrialPeriodsDays { get; set; }

    [JsonProperty("statement_descriptor")]
    public string? StatementDescriptor { get; set; }

    [JsonProperty("name")]
    public string? Name { get; set; }

}