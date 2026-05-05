namespace InventoryAPI.Application.Commands.AddStock;

public record AddStockCommand(Guid ProductId, Guid WarehouseId,int Quantity,int LowStockThreshold = 10);