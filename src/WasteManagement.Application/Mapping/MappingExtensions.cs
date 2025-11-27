using WasteManagement.Application.ViewModels;
using WasteManagement.Domain.Entities;

namespace WasteManagement.Application.Mapping;

public static class MappingExtensions
{
    public static CollectionPointListItemViewModel ToListItemViewModel(this CollectionPoint entity)
        => new()
        {
            Id = entity.Id,
            Code = entity.Code,
            Name = entity.Name,
            Neighborhood = entity.Neighborhood,
            MaterialCategory = entity.MaterialCategory,
            FillPercentage = entity.CapacityKg <= 0 ? 0 : Math.Round((entity.CurrentLoadKg / entity.CapacityKg) * 100, 2),
            IsActive = entity.IsActive,
            HasCriticalLoad = entity.CapacityKg > 0 && entity.CurrentLoadKg / entity.CapacityKg >= 0.85
        };

    public static CollectionPointViewModel ToViewModel(this CollectionPoint entity)
        => new()
        {
            Id = entity.Id,
            Code = entity.Code,
            Name = entity.Name,
            Neighborhood = entity.Neighborhood,
            MaterialCategory = entity.MaterialCategory,
            CapacityKg = entity.CapacityKg,
            CurrentLoadKg = entity.CurrentLoadKg,
            FillPercentage = entity.CapacityKg <= 0 ? 0 : Math.Round((entity.CurrentLoadKg / entity.CapacityKg) * 100, 2),
            SupportsIoTSensors = entity.SupportsIoTSensors,
            IsActive = entity.IsActive,
            Latitude = entity.Latitude,
            Longitude = entity.Longitude,
            LastPickupUtc = entity.LastPickupUtc,
            NextInspectionUtc = entity.NextInspectionUtc,
            Alerts = entity.Alerts.Select(a => a.ToViewModel()).ToList(),
            Requests = entity.Requests.Select(r => r.ToViewModel()).ToList()
        };

    public static WasteAlertViewModel ToViewModel(this WasteAlert entity)
        => new()
        {
            Id = entity.Id,
            CollectionPointId = entity.CollectionPointId,
            Severity = entity.Severity,
            Message = entity.Message,
            TriggeredAtUtc = entity.TriggeredAtUtc,
            Resolved = entity.Resolved,
            ResolvedAtUtc = entity.ResolvedAtUtc
        };

    public static CollectionRequestViewModel ToViewModel(this CollectionRequest entity)
        => new()
        {
            Id = entity.Id,
            CollectionPointId = entity.CollectionPointId,
            Status = entity.Status,
            Priority = entity.Priority,
            EstimatedVolumeKg = entity.EstimatedVolumeKg,
            RequestedAtUtc = entity.RequestedAtUtc,
            ScheduledForUtc = entity.ScheduledForUtc,
            CompletedAtUtc = entity.CompletedAtUtc,
            RequestedBy = entity.RequestedBy,
            Notes = entity.Notes
        };

    public static ImpactSnapshotViewModel ToViewModel(this ImpactSnapshot entity)
        => new()
        {
            Id = entity.Id,
            SnapshotDate = entity.SnapshotDate,
            TotalCollectedKg = entity.TotalCollectedKg,
            Co2AvoidedKg = entity.Co2AvoidedKg,
            ActiveAlerts = entity.ActiveAlerts,
            HouseholdsServed = entity.HouseholdsServed,
            Notes = entity.Notes
        };
}
