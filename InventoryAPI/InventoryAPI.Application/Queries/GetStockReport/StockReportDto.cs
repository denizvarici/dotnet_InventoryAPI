namespace InventoryAPI.Application.Queries.GetStockReport;

public record StockReportDto(
    Guid ProductId,
    string ProductName,
    Guid WarehouseId,
    string WarehouseName,
    int Quantity,
    int LowStockThreshold
    );