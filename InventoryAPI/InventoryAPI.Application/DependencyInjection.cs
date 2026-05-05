using InventoryAPI.Application.Commands.CreateProduct;
using InventoryAPI.Application.Commands.CreateWarehouse;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InventoryAPI.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        //without mediatR we manually register these to services.

        //product handlers
        services.AddScoped<CreateProductCommandHandler>();

        //warehouse handlers
        services.AddScoped<CreateWarehouseCommandHandler>();
        return services;
    }
}