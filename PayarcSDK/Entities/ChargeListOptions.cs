namespace PayarcSDK.Entities;

public class ChargeListOptions
{
    public int? Page { get; init; } = 1;
    public int? Limit { get; init; } = 25;
    
    public string? Search { get; init; }
}