namespace WasteManagement.Application.ViewModels;

public record ImpactSnapshotViewModel
{
    public required Guid Id { get; init; }
    public required DateOnly SnapshotDate { get; init; }
    public required double TotalCollectedKg { get; init; }
    public required double Co2AvoidedKg { get; init; }
    public required int ActiveAlerts { get; init; }
    public required int HouseholdsServed { get; init; }
    public string Notes { get; init; } = string.Empty;
}
