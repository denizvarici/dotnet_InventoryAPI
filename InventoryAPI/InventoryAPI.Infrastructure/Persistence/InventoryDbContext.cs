using InventoryAPI.Domain.Entities;
using InventoryAPI.Domain.Events;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using InventoryAPI.Application.Interfaces;

namespace InventoryAPI.Infrastructure.Persistence
{
    public class InventoryDbContext : DbContext
    {
        private readonly IDomainEventDispatcher? _dispatcher;
        public DbSet<Product> Products { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<StockMovement> StockMovements { get; set; }

        public InventoryDbContext(DbContextOptions<InventoryDbContext> options, IDomainEventDispatcher? dispatcher = null) : base(options)
        {
            _dispatcher = dispatcher;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entitiesWithEvents = ChangeTracker.Entries<IHasDomainEvents>()
                .Where(e => e.Entity.DomainEvents != null && e.Entity.DomainEvents.Any())
                .Select(e => e.Entity)
                .ToList();

            var domainEvents = entitiesWithEvents.SelectMany(e => e.DomainEvents).ToList();
            entitiesWithEvents.ForEach(e => e.ClearDomainEvents());

            var result = await base.SaveChangesAsync(cancellationToken);

            if (_dispatcher != null)
            {
                foreach (var domainEvent in domainEvents)
                {
                    await _dispatcher.DispatchAsync(domainEvent, cancellationToken);
                }
            }

            return result;
        }
    }
}
