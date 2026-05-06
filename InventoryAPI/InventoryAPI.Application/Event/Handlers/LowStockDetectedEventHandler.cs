using InventoryAPI.Application.Interfaces;
using InventoryAPI.Domain.Events;
using Microsoft.Extensions.Logging;
namespace InventoryAPI.Application.Event.Handlers;

public class LowStockDetectedEventHandler : IDomainEventHandler<LowStockDetectedEvent>
{
    private readonly ILogger<LowStockDetectedEventHandler> _logger;

    public LowStockDetectedEventHandler(ILogger<LowStockDetectedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task HandleAsync(LowStockDetectedEvent domainEvent, CancellationToken cancellationToken = default)
    {
        _logger.LogWarning("ALERT: {WarehouseId} on warehouse, {ProductId} product is below threshold! Remaining: {Quantity}",
            domainEvent.WarehouseId, domainEvent.ProductId, domainEvent.CurrentQuantity);

        return Task.CompletedTask;
    }
}