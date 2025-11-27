using WasteManagement.Domain.Enums;

namespace WasteManagement.Application.ViewModels;

public record CollectionPointListItemViewModel
{
    public required Guid Id { get; init; }
    public required string Code { get; init; }
    public required string Name { get; init; }
    public required string Neighborhood { get; init; }
    public required MaterialCategory MaterialCategory { get; init; }
    public required double FillPercentage { get; init; }
    public required bool IsActive { get; init; }
    public bool HasCriticalLoad { get; init; }
}
