namespace InventoryAPI.Domain.Exceptions
{
    public class InsufficientStockException : Exception
    {
        public Guid StockId { get;}
        public int AvailableQuantity { get;}
        public int RequestedAmount { get; }

        public InsufficientStockException(Guid stockId, int availableQuantity, int requestedAmount) : base($"Insufficient stock! StockId: {stockId}, Available: {availableQuantity}, Requested: {requestedAmount}")
        {
            StockId = stockId;
            AvailableQuantity = availableQuantity;
            requestedAmount = requestedAmount;
        }
    }
}
