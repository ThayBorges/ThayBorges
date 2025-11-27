using WasteManagement.Domain.Enums;

namespace WasteManagement.Application.Contracts.Alerts;

public record CreateAlertRequest
{
    public required Guid CollectionPointId { get; init; }
    public required AlertSeverity Severity { get; init; }
    public required string Message { get; init; }
}
