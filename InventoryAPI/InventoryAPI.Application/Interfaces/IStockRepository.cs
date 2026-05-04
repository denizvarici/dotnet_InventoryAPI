using System;
using System.Collections.Generic;
using System.Text;
using InventoryAPI.Domain.Entities;

namespace InventoryAPI.Application.Interfaces
{
    public interface IStockRepository
    {
        Task<Stock?> GetByProductAndWarehouseAsync(Guid productId, Guid warehouseId,
            CancellationToken cancellationToken = default);

        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
