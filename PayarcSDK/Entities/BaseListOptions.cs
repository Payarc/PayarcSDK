namespace PayarcSDK.Entities;

public class BaseListOptions
{
    public virtual int? Page { get; init; } = 1;
    public virtual int? Limit { get; init; } = 25;
    
    public virtual string? Search { get; init; }
}