using Order.Domain.Enum;
using Order.Domain.ValueObject;

namespace Order.Domain.Entity;

public class OrderEntity
{
    private Guid? Id { get; }
    private ClientEntity ClientEntity { get; }
    private List<OrderProductEntity> OrderProducts { get; } = new();
    private Money TotalValue { get; }
    private OrderStatus Status { get; }
    private DateTime CreatedAt { get; } = DateTime.UtcNow;
    private DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    private OrderEntity() {}

    private OrderEntity(Guid? id, ClientEntity clientEntity, List<OrderProductEntity> orderProducts, Money totalValue, OrderStatus status)
    {
        Id = id;
        ClientEntity = clientEntity;
        OrderProducts = orderProducts;
        TotalValue = totalValue;
        Status = status;
    }

    public static OrderEntity Create(Guid? id, ClientEntity clientEntity, List<OrderProductEntity> orderProducts, OrderStatus status)
    {
        if (orderProducts == null || orderProducts.Count == 0)
            throw new ArgumentException("Order must have at least one product");
        
        Money totalValue = Money.Create(0, orderProducts[0].GetUnitPrice().GetCurrency());

        foreach (var orderProduct in orderProducts)
        {
            if (totalValue.GetCurrency() != orderProduct.GetUnitPrice().GetCurrency())
                throw new ArgumentException("All order products must have the same currency");
            
            totalValue = totalValue.Add(orderProduct.GetUnitPrice().Multiply(orderProduct.GetQuantity()));
        }
        
        return new OrderEntity(id, clientEntity, orderProducts, totalValue, status);
    }

    public OrderEntity ChangeStatus(OrderStatus newStatus)
    {
        if ((int)newStatus != (int)Status + 1)
            throw new InvalidOperationException($"Cannot transition from {Status} to {newStatus}. Status must progress sequentially.");

        return new OrderEntity(Id, ClientEntity, OrderProducts, TotalValue, newStatus)
        {
            UpdatedAt = DateTime.UtcNow
        };
    }

    public Guid? GetId() => Id;
    public Guid GetClientId() => ClientEntity.GetId() ?? Guid.Empty;
    public ClientEntity GetClient() => ClientEntity;
    public List<OrderProductEntity> GetOrderProducts() => OrderProducts;
    public Money GetTotalValue() => TotalValue;
    public OrderStatus GetStatus() => Status;
    public DateTime GetCreatedAt() => CreatedAt;
    public DateTime GetUpdatedAt() => UpdatedAt;
}