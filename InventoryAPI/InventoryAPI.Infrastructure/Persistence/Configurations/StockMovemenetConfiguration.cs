using InventoryAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryAPI.Infrastructure.Persistence.Configurations;

public class StockMovementConfiguration : IEntityTypeConfiguration<StockMovement>
{
    public void Configure(EntityTypeBuilder<StockMovement> builder)
    {
        builder.HasKey(sm => sm.Id);

        builder.Property(sm => sm.MovementType)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(sm => sm.CreatedAt)
            .ValueGeneratedOnAdd();
    }
}