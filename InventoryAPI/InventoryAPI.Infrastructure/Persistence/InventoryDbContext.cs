using System.Reflection;
using InventoryAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InventoryAPI.Infrastructure.Persistence
{
    public class InventoryDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<StockMovement> StockMovements { get; set; }

        public InventoryDbContext(DbContextOptions<InventoryDbContext> options) : base(options) 
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
