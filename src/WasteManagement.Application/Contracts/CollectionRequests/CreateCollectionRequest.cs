using WasteManagement.Domain.Enums;

namespace WasteManagement.Application.Contracts.CollectionRequests;

public record CreateCollectionRequest
{
    public required Guid CollectionPointId { get; init; }
    public string RequestedBy { get; init; } = string.Empty;
    public string Priority { get; init; } = "Medium";
    public double EstimatedVolumeKg { get; init; }
    public DateTime? ScheduledForUtc { get; init; }
    public string Notes { get; init; } = string.Empty;
}
