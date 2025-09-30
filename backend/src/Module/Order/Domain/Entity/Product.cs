using Order.Domain.ValueObject;

namespace Order.Domain.Entity;

public class Product
{
    private Guid? Id { get; }
    private ProductName Name { get; }
    private Money Value { get; }
    private DateTime CreatedAt { get; } = DateTime.Now;
    private DateTime UpdatedAt { get; } = DateTime.Now;
    
    private Product(Guid? id, string name, Money value)
    {
        Id = id;
        Name = name;
        Value = value;
    }

    public static Product Create(Guid? id, string name, Money value)
    {
        return new Product(id, name, value);
    }
}