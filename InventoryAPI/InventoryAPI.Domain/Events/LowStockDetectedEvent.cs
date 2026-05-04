namespace InventoryAPI.Domain.Events
{
    public record LowStockDetectedEvent(
        Guid StockId,
        Guid ProductId,
        Guid WarehouseId,
        int CurrentQuantity
    ) : IDomainEvent;
}
