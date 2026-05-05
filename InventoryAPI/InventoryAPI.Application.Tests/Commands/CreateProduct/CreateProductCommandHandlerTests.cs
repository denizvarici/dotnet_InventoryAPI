using FluentAssertions;
using InventoryAPI.Application.Commands.CreateProduct;
using InventoryAPI.Application.Interfaces;
using InventoryAPI.Domain.Entities;
using Moq;

namespace InventoryAPI.Application.Tests.Commands.CreateProduct;

public class CreateProductCommandHandlerTests
{
    private readonly Mock<IProductRepository> _mockRepository;
    private readonly CreateProductCommandHandler _handler;

    public CreateProductCommandHandlerTests()
    {
        _mockRepository = new Mock<IProductRepository>();
        _handler = new CreateProductCommandHandler(_mockRepository.Object);
    }

    [Fact]
    public async Task HandleAsync_Should_Create_Product_And_Call_Repository()
    {
        //arrange, act, assert

        //arrange
        var command = new CreateProductCommand("Excalibur G900", "C-EX-G-900", "Laptop");

        //act
        var resultId = await _handler.HandleAsync(command, CancellationToken.None);

        //assert
        resultId.Should().NotBeEmpty();
        _mockRepository.Verify(repo => repo.AddAsync(
                It.Is<Product>(p => 
                    p.Name == "Excalibur G900" && 
                    p.SKU == "C-EX-G-900" && 
                    p.Category == "Laptop" &&
                    p.Id == resultId),
                It.IsAny<CancellationToken>()),
            Times.Once);

        _mockRepository.Verify(repo => repo.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}