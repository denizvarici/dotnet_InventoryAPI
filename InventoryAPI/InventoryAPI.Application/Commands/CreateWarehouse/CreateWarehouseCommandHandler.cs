using InventoryAPI.Application.Interfaces;
using InventoryAPI.Domain.Entities;

namespace InventoryAPI.Application.Commands.CreateWarehouse;

public class CreateWarehouseCommandHandler : ICommandHandler<CreateWarehouseCommand,Guid>
{
    private IWarehouseRepository _repository;

    public CreateWarehouseCommandHandler(IWarehouseRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> HandleAsync(CreateWarehouseCommand command, CancellationToken cancellationToken = default)
    {
        var warehouse = Warehouse.Create(command.Name, command.Location);

        await _repository.AddAsync(warehouse, cancellationToken);

        await _repository.SaveChangesAsync(cancellationToken);

        return warehouse.Id;
    }
}