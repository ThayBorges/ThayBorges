using Microsoft.EntityFrameworkCore;
using WasteManagement.Domain.Entities;
using WasteManagement.Domain.Enums;
using WasteManagement.Infrastructure.Persistence;

namespace WasteManagement.Infrastructure.Seed;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(WasteManagementDbContext context)
    {
        if (await context.CollectionPoints.AnyAsync())
        {
            return;
        }

        var collectionPoints = new List<CollectionPoint>
        {
            new()
            {
                Code = "CP-001",
                Name = "Hub Central",
                Neighborhood = "Centro",
                MaterialCategory = MaterialCategory.Mixed,
                CapacityKg = 1200,
                CurrentLoadKg = 820,
                SupportsIoTSensors = true,
                Latitude = -23.563210,
                Longitude = -46.654321,
                NextInspectionUtc = DateTime.UtcNow.AddDays(15)
            },
            new()
            {
                Code = "CP-002",
                Name = "Pátio Verde",
                Neighborhood = "Zona Sul",
                MaterialCategory = MaterialCategory.Organic,
                CapacityKg = 800,
                CurrentLoadKg = 640,
                SupportsIoTSensors = false,
                Latitude = -23.600000,
                Longitude = -46.700000,
                NextInspectionUtc = DateTime.UtcNow.AddDays(10)
            },
            new()
            {
                Code = "CP-003",
                Name = "Eco Norte",
                Neighborhood = "Zona Norte",
                MaterialCategory = MaterialCategory.Plastic,
                CapacityKg = 600,
                CurrentLoadKg = 510,
                SupportsIoTSensors = true,
                Latitude = -23.480000,
                Longitude = -46.620000,
                NextInspectionUtc = DateTime.UtcNow.AddDays(20)
            }
        };

        await context.CollectionPoints.AddRangeAsync(collectionPoints);
        await context.SaveChangesAsync();

        var requests = new List<CollectionRequest>
        {
            new()
            {
                CollectionPointId = collectionPoints[0].Id,
                Priority = "High",
                EstimatedVolumeKg = 400,
                RequestedBy = "sala_situacao",
                RequestedAtUtc = DateTime.UtcNow.AddHours(-12)
            },
            new()
            {
                CollectionPointId = collectionPoints[1].Id,
                Priority = "Medium",
                EstimatedVolumeKg = 200,
                RequestedBy = "app_mobilidade",
                RequestedAtUtc = DateTime.UtcNow.AddHours(-30)
            }
        };

        await context.CollectionRequests.AddRangeAsync(requests);

        var alerts = new List<WasteAlert>
        {
            new()
            {
                CollectionPointId = collectionPoints[0].Id,
                Severity = AlertSeverity.High,
                Message = "Carga acima de 80%",
                TriggeredAtUtc = DateTime.UtcNow.AddHours(-2)
            },
            new()
            {
                CollectionPointId = collectionPoints[2].Id,
                Severity = AlertSeverity.Medium,
                Message = "Sensores indicam temperatura elevada",
                TriggeredAtUtc = DateTime.UtcNow.AddHours(-5)
            }
        };

        await context.WasteAlerts.AddRangeAsync(alerts);

        var snapshots = new List<ImpactSnapshot>();
        for (var i = 0; i < 7; i++)
        {
            snapshots.Add(new ImpactSnapshot
            {
                SnapshotDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-i)),
                TotalCollectedKg = 1200 - (i * 50),
                Co2AvoidedKg = 480 - (i * 20),
                ActiveAlerts = i % 3,
                HouseholdsServed = 150 + (i * 5),
                Notes = i == 0 ? "Meta semanal atingida" : string.Empty
            });
        }

        await context.ImpactSnapshots.AddRangeAsync(snapshots);
        await context.SaveChangesAsync();
    }
}
