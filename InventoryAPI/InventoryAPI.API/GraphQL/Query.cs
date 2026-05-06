using InventoryAPI.Application.Interfaces;
using InventoryAPI.Application.Queries.GetLowStockProducts;
using InventoryAPI.Application.Queries.GetStockReport;

namespace InventoryAPI.API.GraphQL;

public class Query
{
    public async Task<IEnumerable<StockReportDto>> GetStockReport(
        [Service]IQueryHandler<GetStockReportQuery,IEnumerable<StockReportDto>> handler,
        CancellationToken cancellationToken
        )
    {
        var query = new GetStockReportQuery();
        return await handler.HandleAsync(query, cancellationToken);
    }

    public async Task<IEnumerable<LowStockProductDto>> GetLowStocks(
        [Service]IQueryHandler<GetLowStockProductsQuery,IEnumerable<LowStockProductDto>> handler,
        CancellationToken cancellationToken
        )
    {
        var query = new GetLowStockProductsQuery();
        return await handler.HandleAsync(query, cancellationToken);
    }
}