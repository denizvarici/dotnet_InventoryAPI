using InventoryAPI.Domain.Enums;

namespace InventoryAPI.Domain.Entities
{
    public class StockMovement
    {
        public Guid Id { get; private set; }
        public Guid ProductId { get; private set; }
        public Guid WarehouseId { get; private set; }
        public int Quantity { get; private set; }
        public MovementType MovementType { get; private set; }
        public Guid ReferenceId { get; private set; }
        public DateTime CreatedAt { get; private set; }

        private StockMovement(){}

        public static StockMovement CreateIn(Guid referenceId,Guid productId, Guid warehouseId, int quantity)
        {
            if (quantity <= 0)
            {
                throw new ArgumentException("Quantity must be greater than zero.", nameof(quantity));
            }
            return new StockMovement
            {
                Id = Guid.NewGuid(),
                ProductId = productId,
                WarehouseId = warehouseId,
                Quantity = quantity,
                MovementType = MovementType.In,
                ReferenceId = referenceId,
                CreatedAt = DateTime.UtcNow
            };
        }

        public static StockMovement CreateOut(Guid referenceId, Guid productId, Guid warehouseId, int quantity)
        {
            if (quantity <= 0)
            {
                throw new ArgumentException("Quantity must be positive system will convert it to negative.", nameof(quantity));
            }
            return new StockMovement
            {
                Id = Guid.NewGuid(),
                ProductId = productId,
                WarehouseId = warehouseId,
                Quantity = -quantity,
                MovementType = MovementType.Out,
                ReferenceId = referenceId,
                CreatedAt = DateTime.UtcNow
            };
        }

        public static StockMovement CreateTransferIn(Guid referenceId, Guid productId, Guid warehouseId, int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("Transfer quantity must be positive.", nameof(quantity));

            return new StockMovement
            {
                Id = Guid.NewGuid(),
                ProductId = productId,
                WarehouseId = warehouseId,
                Quantity = quantity,
                MovementType = MovementType.Transfer,
                ReferenceId = referenceId,
                CreatedAt = DateTime.UtcNow
            };
        }

        public static StockMovement CreateTransferOut(Guid referenceId,Guid productId, Guid warehouseId, int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("Transfer quantity must be positive. system will convert it to negative automaticly.", nameof(quantity));

            return new StockMovement
            {
                Id = Guid.NewGuid(),
                ProductId = productId,
                WarehouseId = warehouseId,
                Quantity = -quantity,
                MovementType = MovementType.Transfer,
                ReferenceId = referenceId,
                CreatedAt = DateTime.UtcNow
            };
        }

    }
}
