using Order.Domain.ValueObject;

namespace Order.Domain.Entity;

public class OrderProductEntity
{
    private Guid? Id { get; }
    private Guid OrderId { get; }
    private Guid ProductId { get; }
    private int Quantity { get; }
    private Money UnitPrice { get; }
    private DateTime CreatedAt { get; } = DateTime.UtcNow;
    private DateTime UpdatedAt { get; } = DateTime.UtcNow;

    // Navigation properties
    private OrderEntity Order { get; }
    private ProductEntity Product { get; }

    private OrderProductEntity() { }

    private OrderProductEntity(Guid? id, Guid orderId, ProductEntity product, int quantity, Money unitPrice)
    {
        Id = id;
        OrderId = orderId;
        Product = product;
        Quantity = quantity;
        UnitPrice = unitPrice;
    }

    public static OrderProductEntity Create(Guid? id, Guid orderId, ProductEntity product, int quantity, Money unitPrice)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero");

        return new OrderProductEntity(id, orderId, product, quantity, unitPrice);
    }
    
    public Guid GetProductId() => Product.GetId() ?? Guid.Empty;
    public int GetQuantity() => Quantity;
    public Money GetUnitPrice() => UnitPrice;
}