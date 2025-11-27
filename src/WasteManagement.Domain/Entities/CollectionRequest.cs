using WasteManagement.Domain.Enums;

namespace WasteManagement.Domain.Entities;

public class CollectionRequest : BaseEntity
{
    public Guid CollectionPointId { get; set; }
    public CollectionPoint? CollectionPoint { get; set; }
    public RequestStatus Status { get; set; } = RequestStatus.Pending;
    public string Priority { get; set; } = "Medium";
    public double EstimatedVolumeKg { get; set; }
    public DateTime RequestedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime? ScheduledForUtc { get; set; }
    public DateTime? CompletedAtUtc { get; set; }
    public string RequestedBy { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
}
