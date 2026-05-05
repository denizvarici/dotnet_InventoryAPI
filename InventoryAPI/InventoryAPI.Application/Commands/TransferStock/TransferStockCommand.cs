namespace InventoryAPI.Application.Commands.TransferStock;

public record TransferStockCommand(
    Guid ProductId,
    Guid FromWarehouseId,
    Guid ToWarehouseId,
    int Quantity
    );