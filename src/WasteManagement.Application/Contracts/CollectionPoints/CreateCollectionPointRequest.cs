using WasteManagement.Domain.Enums;

namespace WasteManagement.Application.Contracts.CollectionPoints;

public record CreateCollectionPointRequest
{
    public required string Code { get; init; }
    public required string Name { get; init; }
    public required string Neighborhood { get; init; }
    public required MaterialCategory MaterialCategory { get; init; }
    public required double CapacityKg { get; init; }
    public double CurrentLoadKg { get; init; }
    public bool SupportsIoTSensors { get; init; }
    public double Latitude { get; init; }
    public double Longitude { get; init; }
    public DateTime? NextInspectionUtc { get; init; }
}
