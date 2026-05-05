using InventoryAPI.Domain.Entities;

namespace InventoryAPI.Application.Interfaces
{
    public interface IStockRepository
    {
        Task AddAsync(Stock stock, CancellationToken cancellationToken = default);
        Task AddMovementAsync(StockMovement stockMovement, CancellationToken cancellationToken = default);
        Task<Stock?> GetByProductAndWarehouseAsync(Guid productId, Guid warehouseId,
            CancellationToken cancellationToken = default);

        Task SaveChangesAsync(CancellationToken cancellationToken = default);



        //transaction management contracts
        Task BeginTransactionAsync(CancellationToken cancellationToken = default);
        Task CommitTransactionAsync(CancellationToken cancellationToken = default);
        Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
    }
}
