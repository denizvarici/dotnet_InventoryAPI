using System;
using System.Collections.Generic;
using System.Text;
using InventoryAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryAPI.Infrastructure.Persistence.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Name).IsRequired().HasMaxLength(200);
            builder.Property(p => p.SKU).IsRequired().HasMaxLength(50);
            builder.Property(p => p.Category).HasMaxLength(100);
            builder.HasIndex(p => p.SKU).IsUnique();
        }
    }
}
