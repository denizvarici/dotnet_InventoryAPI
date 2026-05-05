using InventoryAPI.Application.Interfaces;
using InventoryAPI.Domain.Entities;

namespace InventoryAPI.Application.Commands.CreateProduct;

public class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, Guid>
{
    private readonly IProductRepository _productRepository;

    public CreateProductCommandHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Guid> HandleAsync(CreateProductCommand command, CancellationToken cancellationToken = default)
    {
        var product = Product.Create(command.Name, command.Sku, command.Category);

        await _productRepository.AddAsync(product, cancellationToken);

        await _productRepository.SaveChangesAsync(cancellationToken);

        return product.Id;
    }
}