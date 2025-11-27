using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WasteManagement.Domain.Entities;

namespace WasteManagement.Infrastructure.Persistence.Configurations;

public class WasteAlertConfiguration : IEntityTypeConfiguration<WasteAlert>
{
    public void Configure(EntityTypeBuilder<WasteAlert> builder)
    {
        builder.ToTable("WasteAlerts");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Message)
            .HasMaxLength(240)
            .IsRequired();
    }
}
