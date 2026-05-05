using InventoryAPI.Domain.Entities;

namespace InventoryAPI.Application.Interfaces;

public interface IWarehouseRepository
{
    Task AddAsync(Warehouse warehouse, CancellationToken cancellationToken = default);

    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}