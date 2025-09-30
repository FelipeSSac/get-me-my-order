using Order.Domain.Enum;
using Order.Domain.ValueObject;

namespace Order.Domain.Entity;

public class Order
{
    private Guid? Id { get; }
    private Client Client { get; }
    private Product Product { get; }
    private Money Value { get; }
    private OrderStatus Status { get; }
    private DateTime CreatedAt { get; } = DateTime.Now;
    private DateTime UpdatedAt { get; set; } = DateTime.Now;

    private Order(Guid? id, Client client, Product product, Money value, OrderStatus status)
    {
        Id = id;
        Client = client;
        Product = product;
        Value = value;
        Status = status;
    }

    public static Order Create(Guid? id, Client client, Product product, Money value, OrderStatus status)
    {
        return new Order(id, client, product, value, status);
    }

    public Order ChangeStatus(OrderStatus newStatus)
    {
        if ((int)newStatus != (int)Status + 1)
            throw new InvalidOperationException($"Cannot transition from {Status} to {newStatus}. Status must progress sequentially.");

        return new Order(Id, Client, Product, Value, newStatus)
        {
            UpdatedAt = DateTime.Now
        };
    }
}