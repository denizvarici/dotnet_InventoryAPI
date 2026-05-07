using InventoryAPI.Application.Interfaces;
using InventoryAPI.Domain.Events;
using Microsoft.Extensions.Logging;
using HotChocolate.Subscriptions;
namespace InventoryAPI.Application.Event.Handlers;

public class LowStockDetectedEventHandler : IDomainEventHandler<LowStockDetectedEvent>
{
    private readonly ILogger<LowStockDetectedEventHandler> _logger;
    private readonly ITopicEventSender _eventSender;

    public LowStockDetectedEventHandler(ILogger<LowStockDetectedEventHandler> logger, ITopicEventSender eventSender)
    {
        _logger = logger;
        _eventSender = eventSender;
    }

    public async Task HandleAsync(LowStockDetectedEvent domainEvent, CancellationToken cancellationToken = default)
    {
        var alertMessage = $"ALERT: {domainEvent.WarehouseId} on warehouse, {domainEvent.ProductId} product is below threshold! Remaining: {domainEvent.CurrentQuantity}";
        _logger.LogWarning(alertMessage,
            domainEvent.WarehouseId, domainEvent.ProductId, domainEvent.CurrentQuantity);
        await _eventSender.SendAsync("LowStockAlert", alertMessage, cancellationToken);
    }
}