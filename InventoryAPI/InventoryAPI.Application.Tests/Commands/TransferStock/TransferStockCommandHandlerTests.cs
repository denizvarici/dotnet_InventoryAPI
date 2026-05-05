using FluentAssertions;
using InventoryAPI.Application.Commands.TransferStock;
using InventoryAPI.Application.Interfaces;
using InventoryAPI.Domain.Entities;
using InventoryAPI.Domain.Enums;
using InventoryAPI.Domain.Exceptions;
using Moq;

namespace InventoryAPI.Application.Tests.Commands.TransferStock;

public class TransferStockCommandHandlerTests
{
    private readonly Mock<IStockRepository> _mockRepository;
    private readonly TransferStockCommandHandler _handler;

    public TransferStockCommandHandlerTests()
    {
        _mockRepository = new Mock<IStockRepository>();
        _handler = new TransferStockCommandHandler(_mockRepository.Object);
    }

    [Fact]
    public async Task HandleAsync_Should_Transfer_Stock_Successfully_And_Commit_Transaction()
    {
        //arrange
        var productId = Guid.NewGuid();
        var fromWarehouseId = Guid.NewGuid();
        var toWarehouseId = Guid.NewGuid();
        var quantityToTransfer = 10;
        var command = new TransferStockCommand(productId, fromWarehouseId, toWarehouseId, quantityToTransfer);

        var fromStock = Stock.Create(productId, fromWarehouseId, 50);
        var toStock = Stock.Create(productId, toWarehouseId, 5);

        _mockRepository.Setup(repo => repo.GetByProductAndWarehouseAsync(
            productId, fromWarehouseId, It.IsAny<CancellationToken>()
        )).ReturnsAsync(fromStock);

        _mockRepository.Setup(repo => repo.GetByProductAndWarehouseAsync(
            productId, toWarehouseId, It.IsAny<CancellationToken>()
        )).ReturnsAsync(toStock);

        var capturedMovements = new List<StockMovement>();
        _mockRepository.Setup(repo => repo.AddMovementAsync(It.IsAny<StockMovement>(), It.IsAny<CancellationToken>()))
            .Callback((StockMovement sm, CancellationToken ct) => capturedMovements.Add(sm))
            .Returns(Task.CompletedTask);

        //act
        await _handler.HandleAsync(command,CancellationToken.None);

        //assert
        fromStock.Quantity.Should().Be(40);
        toStock.Quantity.Should().Be(15);

        capturedMovements.Should().HaveCount(2);

        var outMovement = capturedMovements.Single(m => m.WarehouseId == fromWarehouseId);
        outMovement.Quantity.Should().Be(-10);
        outMovement.MovementType.Should().Be(MovementType.Transfer);

        var inMovement = capturedMovements.Single(m => m.WarehouseId == toWarehouseId);
        inMovement.Quantity.Should().Be(10);
        inMovement.MovementType.Should().Be(MovementType.Transfer);

        outMovement.ReferenceId.Should().Be(inMovement.ReferenceId);
        outMovement.ReferenceId.Should().NotBeEmpty();

        _mockRepository.Verify(repo => repo.BeginTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
        _mockRepository.Verify(repo => repo.CommitTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
        _mockRepository.Verify(repo => repo.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);


    }


    [Fact]
    public async Task HandleAsync_Should_Rollback_Transaction_When_Stock_Is_Insufficient()
    {
        var productId = Guid.NewGuid();
        var fromWarehouseId = Guid.NewGuid();
        var toWarehouseId = Guid.NewGuid();
        var quantityToTransfer = 100; 

        var command = new TransferStockCommand(productId, fromWarehouseId, toWarehouseId, quantityToTransfer);

        var fromStock = Stock.Create(productId, fromWarehouseId, 50);

        _mockRepository.Setup(repo => repo.GetByProductAndWarehouseAsync(
            productId, fromWarehouseId, It.IsAny<CancellationToken>()
        )).ReturnsAsync(fromStock);

        Func<Task> act = async () => await _handler.HandleAsync(command, CancellationToken.None);

        await act.Should().ThrowAsync<InsufficientStockException>();

        _mockRepository.Verify(repo => repo.BeginTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);

        _mockRepository.Verify(repo => repo.RollbackTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);

        _mockRepository.Verify(repo => repo.CommitTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);
        _mockRepository.Verify(repo => repo.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        _mockRepository.Verify(repo => repo.AddMovementAsync(It.IsAny<StockMovement>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}