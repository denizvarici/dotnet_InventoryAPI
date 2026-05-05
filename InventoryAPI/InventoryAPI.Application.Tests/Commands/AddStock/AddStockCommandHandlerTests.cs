using InventoryAPI.Application.Commands.AddStock;
using InventoryAPI.Application.Interfaces;
using InventoryAPI.Domain.Entities;
using Moq;

namespace InventoryAPI.Application.Tests.Commands.AddStock;

public class AddStockCommandHandlerTests
{
    private readonly Mock<IStockRepository> _mockRepository;
    private readonly AddStockCommandHandler _handler;

    public AddStockCommandHandlerTests()
    {
        _mockRepository = new Mock<IStockRepository>();
        _handler = new AddStockCommandHandler(_mockRepository.Object);
    }

    [Fact]
    public async Task HandleAsync_WhenStockDoesNotExists_ShouldCreateNewStockAndStockMovement()
    {
        // arrange, act, assert

        //arrange - hazırlık aşaması 
        var productId = Guid.NewGuid();
        var warehouseId = Guid.NewGuid();
        var quantity = 50;

        var command = new AddStockCommand(productId, warehouseId, quantity);

        _mockRepository.Setup(repo => repo.GetByProductAndWarehouseAsync(
            productId,
            warehouseId,
            It.IsAny<CancellationToken>()
        )).ReturnsAsync((Stock?)null);



        //act - eyleme geçme aşaması
        await _handler.HandleAsync(command, CancellationToken.None);

        //assert - doğrulama aşaması

        //is new stock created ?
        _mockRepository.Verify(repo=>repo.AddAsync(
            It.Is<Stock>(s=>s.ProductId == productId && s.WarehouseId == warehouseId && s.Quantity == quantity),
            It.IsAny<CancellationToken>()
            ),Times.Once);

        //is stock movement added ?
        _mockRepository.Verify(repo=>repo.AddMovementAsync(
            It.Is<StockMovement>(sm=>sm.ProductId == productId && sm.WarehouseId == warehouseId && sm.Quantity == quantity),
            It.IsAny<CancellationToken>()
            ),Times.Once);

        //is changes saved ?
        _mockRepository.Verify(repo=>repo.SaveChangesAsync(It.IsAny<CancellationToken>()),Times.Once);
    }
}