using Microsoft.Extensions.Configuration;
using Order.Domain.Event;
using Order.Infrastructure.Messaging;

namespace Order.Application.EventHandler;

public class OrderCreatedEventHandler : IEventHandler<OrderCreatedEvent>
{
    private readonly IServiceBusClient _serviceBusClient;
    private readonly string _queueName;

    public OrderCreatedEventHandler(IServiceBusClient serviceBusClient, IConfiguration configuration)
    {
        _serviceBusClient = serviceBusClient;
        _queueName = configuration["SERVICEBUS_QUEUE_NAME"]
                     ?? throw new InvalidOperationException("Service Bus queue name is not configured");
    }

    public async Task HandleAsync(OrderCreatedEvent domainEvent)
    {
        await _serviceBusClient.SendMessageAsync(domainEvent, _queueName);
    }
}