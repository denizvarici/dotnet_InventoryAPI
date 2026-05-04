using InventoryAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryAPI.Infrastructure.Persistence.Configurations
{
    public class StockConfiguration : IEntityTypeConfiguration<Stock>
    {
        public void Configure(EntityTypeBuilder<Stock> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Ignore(s => s.DomainEvents);

            builder.ToTable(t => t.HasCheckConstraint("CK_Stock_Quantity_NonNegative", "\"Quantity\" >= 0"));

            builder.HasOne<Product>()
                .WithMany()
                .HasForeignKey(s => s.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
