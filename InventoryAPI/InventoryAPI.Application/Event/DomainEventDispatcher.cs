using InventoryAPI.Application.Interfaces;
using InventoryAPI.Domain.Events;
using Microsoft.Extensions.DependencyInjection;

namespace InventoryAPI.Application.Event;

public class DomainEventDispatcher : IDomainEventDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    public DomainEventDispatcher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task DispatchAsync(IDomainEvent domainEvent, CancellationToken cancellationToken = default)
    {
        var eventType = domainEvent.GetType();

        var handlerType = typeof(IDomainEventHandler<>).MakeGenericType(eventType);

        var handlers = _serviceProvider.GetServices(handlerType);

        foreach (var handler in handlers)
        {
            if (handler != null)
            {
                var method = handler.GetType().GetMethod("HandleAsync");
                if (method != null)
                {
                    await(Task)method.Invoke(handler, new object[] { domainEvent, cancellationToken });
                }
            }
        }
    }
}