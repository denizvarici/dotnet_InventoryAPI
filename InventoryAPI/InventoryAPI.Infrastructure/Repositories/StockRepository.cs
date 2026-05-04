using InventoryAPI.Application.Interfaces;
using InventoryAPI.Domain.Entities;
using InventoryAPI.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace InventoryAPI.Infrastructure.Repositories;

public class StockRepository : IStockRepository
{
    private readonly InventoryDbContext _context;

    public StockRepository(InventoryDbContext context)
    {
        _context = context;
    }

    public async Task<Stock?> GetByProductAndWarehouseAsync(Guid productId, Guid warehouseId, CancellationToken cancellationToken = default)
    {
        return await _context.Stocks
            .FirstOrDefaultAsync(s => s.ProductId == productId && s.WarehouseId == warehouseId, cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}