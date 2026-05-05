using InventoryAPI.Domain.Entities;

namespace InventoryAPI.Application.Interfaces;

public interface IProductRepository
{
    Task AddAsync(Product product,CancellationToken cancellationToken=default);

    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}