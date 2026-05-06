using InventoryAPI.Application.Interfaces;
using Dapper;

namespace InventoryAPI.Application.Queries.GetLowStockProducts;

public class GetLowStockProductsQueryHandler : IQueryHandler<GetLowStockProductsQuery,IEnumerable<LowStockProductDto>>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public GetLowStockProductsQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<IEnumerable<LowStockProductDto>> HandleAsync(GetLowStockProductsQuery query, CancellationToken cancellationToken = default)
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
            INNER JOIN ""Warehouses"" w ON s.""WarehouseId"" = w.""Id""
            WHERE s.""Quantity"" <= s.""LowStockThreshold"";
        ";

        using var connection = _sqlConnectionFactory.CreateConnection();
        var result = await connection.QueryAsync<LowStockProductDto>(sql);
        return result;
    }
}