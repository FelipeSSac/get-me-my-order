namespace Order.Domain.ValueObject;

public sealed class ProductName : IEquatable<ProductName>
{
    private string Value { get; }

    private ProductName(string value)
    {
        Value = value;
    }

    public static ProductName Create(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Product name cannot be empty", nameof(name));

        name = name.Trim();

        if (name.Length < 3)
            throw new ArgumentException("Product name must be at least 3 characters", nameof(name));

        if (name.Length > 100)
            throw new ArgumentException("Product name cannot exceed 100 characters", nameof(name));

        return new ProductName(name);
    }

    public static implicit operator ProductName(string name) => Create(name);

    public static implicit operator string(ProductName productName) => productName.Value;

    public bool Equals(ProductName? other)
    {
        if (other is null) return false;
        return Value == other.Value;
    }

    public override bool Equals(object? obj) => obj is ProductName other && Equals(other);

    public override int GetHashCode() => Value.GetHashCode();

    public override string ToString() => Value;

    public static bool operator ==(ProductName? left, ProductName? right) =>
        left is null ? right is null : left.Equals(right);

    public static bool operator !=(ProductName? left, ProductName? right) => !(left == right);
}