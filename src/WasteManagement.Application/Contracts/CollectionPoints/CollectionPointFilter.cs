using WasteManagement.Domain.Enums;

namespace WasteManagement.Application.Contracts.CollectionPoints;

public record CollectionPointFilter
{
    public string? Neighborhood { get; init; }
    public MaterialCategory? Material { get; init; }
    public bool? OnlyActive { get; init; }
    public bool? OnlyCriticalLoad { get; init; }
    public double? MinFillPercentage { get; init; }
}
