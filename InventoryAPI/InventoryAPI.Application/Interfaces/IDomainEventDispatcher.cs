using InventoryAPI.Domain.Events;

namespace InventoryAPI.Application.Interfaces;

public interface IDomainEventDispatcher
{
    Task DispatchAsync(IDomainEvent domainEvent, CancellationToken cancellationToken = default);
}