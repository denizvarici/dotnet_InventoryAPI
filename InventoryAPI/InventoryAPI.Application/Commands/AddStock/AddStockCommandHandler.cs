using InventoryAPI.Application.Interfaces;
using InventoryAPI.Domain.Entities;

namespace InventoryAPI.Application.Commands.AddStock;

public class AddStockCommandHandler : ICommandHandler<AddStockCommand,Guid>
{
    private readonly IStockRepository _repository;

    public AddStockCommandHandler(IStockRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> HandleAsync(AddStockCommand command, CancellationToken cancellationToken = default)
    {
        var stock =
            await _repository.GetByProductAndWarehouseAsync(command.ProductId, command.WarehouseId, cancellationToken);

        if (stock == null)
        {
            stock = Stock.Create(command.ProductId, command.WarehouseId, command.Quantity, command.LowStockThreshold);
            await _repository.AddAsync(stock, cancellationToken);
        }
        else
        {
            stock.Increase(command.Quantity);
        }

        var referenceId = Guid.NewGuid();

        var stockMovement = StockMovement.CreateIn(referenceId, command.ProductId, command.WarehouseId, command.Quantity);

        await _repository.AddMovementAsync(stockMovement, cancellationToken);

        await _repository.SaveChangesAsync(cancellationToken);

        return stock.Id;
    }
}