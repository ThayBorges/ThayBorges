using WasteManagement.Domain.Enums;

namespace WasteManagement.Domain.Entities;

public class WasteAlert : BaseEntity
{
    public Guid CollectionPointId { get; set; }
    public CollectionPoint? CollectionPoint { get; set; }
    public AlertSeverity Severity { get; set; }
    public string Message { get; set; } = string.Empty;
    public DateTime TriggeredAtUtc { get; set; } = DateTime.UtcNow;
    public bool Resolved { get; set; }
    public DateTime? ResolvedAtUtc { get; set; }
}
