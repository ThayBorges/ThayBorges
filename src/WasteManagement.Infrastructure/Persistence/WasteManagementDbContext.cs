using Microsoft.EntityFrameworkCore;
using WasteManagement.Domain.Entities;

namespace WasteManagement.Infrastructure.Persistence;

public class WasteManagementDbContext(DbContextOptions<WasteManagementDbContext> options)
    : DbContext(options)
{
    public DbSet<CollectionPoint> CollectionPoints => Set<CollectionPoint>();
    public DbSet<CollectionRequest> CollectionRequests => Set<CollectionRequest>();
    public DbSet<WasteAlert> WasteAlerts => Set<WasteAlert>();
    public DbSet<ImpactSnapshot> ImpactSnapshots => Set<ImpactSnapshot>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(WasteManagementDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
