using Order.Application.EventHandler.Interface;
using Order.Domain.Event;

namespace Order.Application.EventHandler;

public class OrderStatusChangedEventHandler : IEventHandler<OrderStatusChangedEvent>
{
    public async Task HandleAsync(OrderStatusChangedEvent domainEvent)
    {
    }
}