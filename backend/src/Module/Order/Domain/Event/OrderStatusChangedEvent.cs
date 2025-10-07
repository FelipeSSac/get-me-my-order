using Order.Domain.Enum;

namespace Order.Domain.Event;

public class OrderStatusChangedEvent
{
    public Guid OrderId { get; }
    public Guid ClientId { get; }
    public decimal TotalValue { get; }
    public string Currency { get; }
    public OrderStatus OldStatus { get; }
    public OrderStatus NewStatus { get; }
    public DateTime CreatedAt { get; }

    public OrderStatusChangedEvent(
        Guid orderId, 
        Guid clientId, 
        decimal totalValue, 
        string currency, 
        OrderStatus oldStatus, 
        OrderStatus newStatus, 
        DateTime createdAt
    ) {
        OrderId = orderId;
        ClientId = clientId;
        TotalValue = totalValue;
        Currency = currency;
        OldStatus = oldStatus;
        NewStatus = newStatus;
        CreatedAt = createdAt;
    }
}