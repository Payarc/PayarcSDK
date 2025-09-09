namespace PayarcSDK.Entities;

public class BaseListOptions
{
    public virtual int? Page { get; init; } = 1;
    public virtual int? Limit { get; init; } = 25;
    
    public virtual string? Search { get; init; }

	public string? Report_DateGTE { get; init; }
	public string? Report_DateLTE { get; init; }

	public string? From_Date { get; init; }
	public string? To_Date { get; init; }
}