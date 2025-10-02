namespace Order.Domain.Event;

public class OrderCreatedEvent
{
    public Guid OrderId { get; }
    public Guid ClientId { get; }
    public decimal TotalValue { get; }
    public string Currency { get; }
    public DateTime CreatedAt { get; }

    public OrderCreatedEvent(Guid orderId, Guid clientId, decimal totalValue, string currency, DateTime createdAt)
    {
        OrderId = orderId;
        ClientId = clientId;
        TotalValue = totalValue;
        Currency = currency;
        CreatedAt = createdAt;
    }
}