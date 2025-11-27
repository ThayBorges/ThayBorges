namespace WasteManagement.Application.Contracts.Impact;

public record ImpactReportQuery
{
    public DateOnly? Start { get; init; }
    public DateOnly? End { get; init; }
}
