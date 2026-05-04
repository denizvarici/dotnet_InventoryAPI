using InventoryAPI.Domain.Events;
using InventoryAPI.Domain.Exceptions;

namespace InventoryAPI.Domain.Entities
{
    public class Stock
    {
        public Guid Id { get; private set; }
        public Guid ProductId { get; private set; }
        public Guid WarehouseId { get; private set; }
        public int Quantity { get; private set; }
        public int ReservedQty { get; private set; }
        public int LowStockThreshold { get; private set; }
        public DateTime LastUpdated { get; private set; }


        private readonly List<IDomainEvent> _events = new();
        public IReadOnlyList<IDomainEvent> DomainEvents => _events.AsReadOnly();

        private Stock(){}

        public static Stock Create(Guid productId, Guid warehouseId, int lowStockThreshold = 10, int initialQuantity = 0)
        {
            return new Stock()
            {
                Id = Guid.NewGuid(),
                ProductId = productId,
                WarehouseId = warehouseId,
                LowStockThreshold = lowStockThreshold,
                Quantity = initialQuantity,
                LastUpdated = DateTime.UtcNow
            };
        }

        public void Decrease(int amount)
        {
            if (amount > Quantity)
            {
                throw new InsufficientStockException(Id, Quantity, amount);
            }

            Quantity -= amount;
            LastUpdated = DateTime.UtcNow;

            if (Quantity <= LowStockThreshold)
            {
                _events.Add(new LowStockDetectedEvent(Id,ProductId,WarehouseId,Quantity));
            }
        }
        
        public void Increase(int amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("Amount must be positive.");
            }
            Quantity += amount;
            LastUpdated = DateTime.UtcNow;
        }

    }
}
