namespace InventoryAPI.Application.Queries.GetLowStockProducts;

public record LowStockProductDto(
    Guid ProductId,
    string ProductName,
    Guid WarehouseId,
    string WarehouseName,
    int Quantity,
    int LowStockThreshold);