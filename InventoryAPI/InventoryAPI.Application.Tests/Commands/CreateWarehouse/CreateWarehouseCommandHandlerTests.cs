using FluentAssertions;
using InventoryAPI.Application.Commands.CreateWarehouse;
using InventoryAPI.Application.Interfaces;
using InventoryAPI.Domain.Entities;
using Moq;

namespace InventoryAPI.Application.Tests.Commands.CreateWarehouse;

public class CreateWarehouseCommandHandlerTests
{
    private readonly Mock<IWarehouseRepository> _mockRepository;
    private readonly CreateWarehouseCommandHandler _handler;

    public CreateWarehouseCommandHandlerTests()
    {
        _mockRepository = new Mock<IWarehouseRepository>();
        _handler = new CreateWarehouseCommandHandler(_mockRepository.Object);
    }

    [Fact]
    public async Task HandleAsync_Should_Create_Warehouse_And_Call_Repository()
    {
        //arrange, act, assert

        //arrange
        var command = new CreateWarehouseCommand("Merkez Depo", "Istanbul / Esenyurt");

        //act
        var resultId = await _handler.HandleAsync(command,CancellationToken.None);

        //assert
        resultId.Should().NotBeEmpty();

        _mockRepository.Verify(repo=>repo.AddAsync(
            It.Is<Warehouse>(w=>
                w.Name == "Merkez Depo" &&
                w.Location == "Istanbul / Esenyurt" &&
                w.IsActive == true &&
                w.Stocks.Count == 0
                ),
            It.IsAny<CancellationToken>()
            ),Times.Once);

        _mockRepository.Verify(repo=>repo.SaveChangesAsync(It.IsAny<CancellationToken>()),Times.Once);
    }
}