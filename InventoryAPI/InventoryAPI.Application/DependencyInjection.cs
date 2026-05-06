using InventoryAPI.Application.Commands.AddStock;
using InventoryAPI.Application.Commands.CreateProduct;
using InventoryAPI.Application.Commands.CreateWarehouse;
using InventoryAPI.Application.Commands.TransferStock;
using InventoryAPI.Application.Interfaces;
using InventoryAPI.Application.Queries.GetLowStockProducts;
using InventoryAPI.Application.Queries.GetStockReport;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InventoryAPI.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        //command handlers registration
        services.AddScoped<ICommandHandler<CreateProductCommand, Guid>, CreateProductCommandHandler>();
        services.AddScoped<ICommandHandler<CreateWarehouseCommand, Guid>, CreateWarehouseCommandHandler>();
        services.AddScoped<ICommandHandler<AddStockCommand, Guid>, AddStockCommandHandler>();
        services.AddScoped<ICommandHandler<TransferStockCommand, Guid>, TransferStockCommandHandler>();

        //query handlers registrationn
        services.AddScoped<IQueryHandler<GetStockReportQuery, IEnumerable<StockReportDto>>, GetStockReportQueryHandler>();
        services.AddScoped<IQueryHandler<GetLowStockProductsQuery, IEnumerable<LowStockProductDto>>, GetLowStockProductsQueryHandler>();
        return services;
    }
}