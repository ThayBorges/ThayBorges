using WasteManagement.Domain.Enums;

namespace WasteManagement.Application.ViewModels;

public record WasteAlertViewModel
{
    public required Guid Id { get; init; }
    public required Guid CollectionPointId { get; init; }
    public required AlertSeverity Severity { get; init; }
    public required string Message { get; init; }
    public required DateTime TriggeredAtUtc { get; init; }
    public bool Resolved { get; init; }
    public DateTime? ResolvedAtUtc { get; init; }
}
