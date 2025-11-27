using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WasteManagement.Domain.Entities;

namespace WasteManagement.Infrastructure.Persistence.Configurations;

public class ImpactSnapshotConfiguration : IEntityTypeConfiguration<ImpactSnapshot>
{
    public void Configure(EntityTypeBuilder<ImpactSnapshot> builder)
    {
        builder.ToTable("ImpactSnapshots");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Notes)
            .HasMaxLength(240);
    }
}
