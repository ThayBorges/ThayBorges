using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WasteManagement.Domain.Entities;

namespace WasteManagement.Infrastructure.Persistence.Configurations;

public class CollectionRequestConfiguration : IEntityTypeConfiguration<CollectionRequest>
{
    public void Configure(EntityTypeBuilder<CollectionRequest> builder)
    {
        builder.ToTable("CollectionRequests");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Priority)
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(x => x.Notes)
            .HasMaxLength(400);

        builder.Property(x => x.EstimatedVolumeKg)
            .HasPrecision(18, 2);
    }
}
