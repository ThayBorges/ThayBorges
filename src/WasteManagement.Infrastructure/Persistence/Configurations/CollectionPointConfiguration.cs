using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WasteManagement.Domain.Entities;

namespace WasteManagement.Infrastructure.Persistence.Configurations;

public class CollectionPointConfiguration : IEntityTypeConfiguration<CollectionPoint>
{
    public void Configure(EntityTypeBuilder<CollectionPoint> builder)
    {
        builder.ToTable("CollectionPoints");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Code)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(x => x.Name)
            .HasMaxLength(120)
            .IsRequired();

        builder.Property(x => x.Neighborhood)
            .HasMaxLength(120)
            .IsRequired();

        builder.Property(x => x.CapacityKg)
            .HasPrecision(18, 2);

        builder.Property(x => x.CurrentLoadKg)
            .HasPrecision(18, 2);

        builder.HasMany(x => x.Requests)
            .WithOne(x => x.CollectionPoint)
            .HasForeignKey(x => x.CollectionPointId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Alerts)
            .WithOne(x => x.CollectionPoint)
            .HasForeignKey(x => x.CollectionPointId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.Code)
            .IsUnique();
    }
}
