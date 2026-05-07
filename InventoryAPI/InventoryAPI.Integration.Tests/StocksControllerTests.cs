using System.Net.Http.Json;
using FluentAssertions;
using InventoryAPI.Application.Commands.AddStock;
using InventoryAPI.Domain.Entities;
using InventoryAPI.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace InventoryAPI.Integration.Tests;

public class StocksControllerTests : IClassFixture<InventoryApiFactory>
{
    private readonly HttpClient _client;
    private readonly InventoryApiFactory _factory;

    public StocksControllerTests(InventoryApiFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task AddStock_WhenCalled_ShouldAddStockToDatabaseAndReturnOk()
    {
        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<InventoryDbContext>();

        var testProduct = Product.Create("Test Ürünü", "TST-01", "Kategori");
        var testWarehouse = Warehouse.Create("Test Deposu", "Lokasyon");

        dbContext.Products.Add(testProduct);
        dbContext.Warehouses.Add(testWarehouse);
        await dbContext.SaveChangesAsync();

        var command = new AddStockCommand(testProduct.Id, testWarehouse.Id, 50, 10);

        var response = await _client.PostAsJsonAsync("/api/stocks/add", command);

        // var errorContent = await response.Content.ReadAsStringAsync();

        response.IsSuccessStatusCode.Should().BeTrue();

        var savedStock = await dbContext.Stocks
            .FirstOrDefaultAsync(s => s.ProductId == command.ProductId && s.WarehouseId == command.WarehouseId);

        savedStock.Should().NotBeNull();
        savedStock!.Quantity.Should().Be(50);
        savedStock.LowStockThreshold.Should().Be(10);
    }
}