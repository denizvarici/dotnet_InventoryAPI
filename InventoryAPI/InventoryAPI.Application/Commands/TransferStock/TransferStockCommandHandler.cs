using InventoryAPI.Application.Interfaces;
using InventoryAPI.Domain.Entities;

namespace InventoryAPI.Application.Commands.TransferStock;

public class TransferStockCommandHandler : ICommandHandler<TransferStockCommand,Guid>
{
    private readonly IStockRepository _repository;

    public TransferStockCommandHandler(IStockRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> HandleAsync(TransferStockCommand command, CancellationToken cancellationToken = default)
    {
        await _repository.BeginTransactionAsync(cancellationToken);

        try
        {
            var fromStock = await _repository.GetByProductAndWarehouseAsync(command.ProductId, command.FromWarehouseId,cancellationToken);
            if (fromStock == null)
            {
                throw new Exception("This product doesn't exists in source warehouse.");
            }

            var toStock = await _repository.GetByProductAndWarehouseAsync(command.ProductId, command.ToWarehouseId,cancellationToken);

            fromStock.Decrease(command.Quantity);

            if (toStock == null)
            {
                var stock = Stock.Create(command.ProductId, command.ToWarehouseId, command.Quantity);
                await _repository.AddAsync(stock, cancellationToken);
            }
            else
            {
                toStock.Increase(command.Quantity);
            }

            //stock movement logging codes
            var transferReferenceId = Guid.NewGuid();
            var outMovement = StockMovement.CreateTransferOut(transferReferenceId, command.ProductId,
                command.FromWarehouseId, command.Quantity);
            var inMovement = StockMovement.CreateTransferIn(transferReferenceId, command.ProductId,
                command.ToWarehouseId, command.Quantity);

            await _repository.AddMovementAsync(outMovement, cancellationToken);
            await _repository.AddMovementAsync(inMovement, cancellationToken);

            await _repository.SaveChangesAsync(cancellationToken);
            await _repository.CommitTransactionAsync(cancellationToken);
            return transferReferenceId;
        }
        catch
        {
            await _repository.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }
}