using WasteManagement.Domain.Enums;

namespace WasteManagement.Application.ViewModels;

public record CollectionRequestViewModel
{
    public required Guid Id { get; init; }
    public required Guid CollectionPointId { get; init; }
    public required RequestStatus Status { get; init; }
    public required string Priority { get; init; }
    public required double EstimatedVolumeKg { get; init; }
    public required DateTime RequestedAtUtc { get; init; }
    public DateTime? ScheduledForUtc { get; init; }
    public DateTime? CompletedAtUtc { get; init; }
    public string RequestedBy { get; init; } = string.Empty;
    public string Notes { get; init; } = string.Empty;
}
