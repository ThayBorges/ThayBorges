namespace WasteManagement.Domain.Entities;

public class ImpactSnapshot : BaseEntity
{
    public DateOnly SnapshotDate { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);
    public double TotalCollectedKg { get; set; }
    public double Co2AvoidedKg { get; set; }
    public int ActiveAlerts { get; set; }
    public int HouseholdsServed { get; set; }
    public string Notes { get; set; } = string.Empty;
}
