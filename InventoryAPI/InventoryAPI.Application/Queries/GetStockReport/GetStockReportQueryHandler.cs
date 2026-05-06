using Dapper;
using InventoryAPI.Application.Interfaces;

namespace InventoryAPI.Application.Queries.GetStockReport;

public class GetStockReportQueryHandler : IQueryHandler<GetStockReportQuery,IEnumerable<StockReportDto>>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public GetStockReportQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<IEnumerable<StockReportDto>> HandleAsync(GetStockReportQuery query, CancellationToken cancellationToken = default)
    {
        const string sql = @"
            SELECT 
                p.""Id"" AS ProductId, 
                p.""Name"" AS ProductName, 
                w.""Id"" AS WarehouseId, 
                w.""Name"" AS WarehouseName, 
                s.""Quantity"",
                s.""LowStockThreshold""
            FROM ""Stocks"" s
            INNER JOIN ""Products"" p ON s.""ProductId"" = p.""Id""
            INNER JOIN ""Warehouses"" w ON s.""WarehouseId"" = w.""Id"";
        ";

        using var connection = _sqlConnectionFactory.CreateConnection();

        var result = await connection.QueryAsync<StockReportDto>(sql);

        return result;
    }
}