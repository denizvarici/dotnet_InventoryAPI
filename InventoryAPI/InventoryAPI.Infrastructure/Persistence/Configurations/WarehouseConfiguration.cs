using InventoryAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryAPI.Infrastructure.Persistence.Configurations;

public class WarehouseConfiguration : IEntityTypeConfiguration<Warehouse>
{
    public void Configure(EntityTypeBuilder<Warehouse> builder)
    {
        builder.HasKey(w => w.Id);

        builder.Property(w => w.Name).IsRequired().HasMaxLength(200);
        builder.Property(w => w.Location).HasMaxLength(500);

        builder.HasMany(w => w.Stocks)
            .WithOne()
            .HasForeignKey(s => s.WarehouseId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}