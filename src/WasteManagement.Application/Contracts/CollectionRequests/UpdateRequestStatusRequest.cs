using WasteManagement.Domain.Enums;

namespace WasteManagement.Application.Contracts.CollectionRequests;

public record UpdateRequestStatusRequest
{
    public required RequestStatus Status { get; init; }
    public DateTime? CompletedAtUtc { get; init; }
    public string Notes { get; init; } = string.Empty;
}
