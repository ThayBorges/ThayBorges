using WasteManagement.Domain.Enums;

namespace WasteManagement.Application.ViewModels;

public record CollectionPointViewModel
{
    public required Guid Id { get; init; }
    public required string Code { get; init; }
    public required string Name { get; init; }
    public required string Neighborhood { get; init; }
    public required MaterialCategory MaterialCategory { get; init; }
    public required double CapacityKg { get; init; }
    public required double CurrentLoadKg { get; init; }
    public required double FillPercentage { get; init; }
    public required bool SupportsIoTSensors { get; init; }
    public required bool IsActive { get; init; }
    public required double Latitude { get; init; }
    public required double Longitude { get; init; }
    public DateTime? LastPickupUtc { get; init; }
    public DateTime? NextInspectionUtc { get; init; }
    public IReadOnlyCollection<WasteAlertViewModel> Alerts { get; init; } = Array.Empty<WasteAlertViewModel>();
    public IReadOnlyCollection<CollectionRequestViewModel> Requests { get; init; } = Array.Empty<CollectionRequestViewModel>();
}
