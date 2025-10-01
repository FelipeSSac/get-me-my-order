using Order.Domain.ValueObject;

namespace Order.Domain.Entity;

public class ProductEntity
{
    private Guid? Id { get; }
    private ProductName Name { get; }
    private Money Value { get; }
    private DateTime CreatedAt { get; } = DateTime.UtcNow;
    private DateTime UpdatedAt { get; } = DateTime.UtcNow;
    
    private ProductEntity() {}
    
    private ProductEntity(Guid? id, string name, Money value)
    {
        Id = id;
        Name = name;
        Value = value;
    }

    public static ProductEntity Create(Guid? id, string name, Money value)
    {
        return new ProductEntity(id, name, value);
    }

    public Guid? GetId() => Id;
    public string GetName() => Name;
    public Money GetValue() => Value;
}