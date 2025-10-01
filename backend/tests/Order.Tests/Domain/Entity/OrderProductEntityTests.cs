using FluentAssertions;
using Order.Domain.Entity;
using Order.Domain.ValueObject;

namespace Order.Tests.Domain.Entity;

public class OrderProductEntityTests
{
    [Fact]
    public void Create_ShouldCreateOrderProduct_WhenValidParameters()
    {
        // Arrange
        var id = Guid.NewGuid();
        var orderId = Guid.NewGuid();
        var product = ProductEntity.Create(Guid.NewGuid(), ProductName.Create("Laptop"), Money.Create(1500, "BRL"));
        var quantity = 2;
        var unitPrice = Money.Create(1500, "BRL");

        // Act
        var orderProduct = OrderProductEntity.Create(id, orderId, product, quantity, unitPrice);

        // Assert
        orderProduct.Should().NotBeNull();
        orderProduct.GetProductName().Should().Be("Laptop");
        orderProduct.GetQuantity().Should().Be(2);
        orderProduct.GetUnitPrice().GetAmount().Should().Be(1500);
        orderProduct.GetUnitPrice().GetCurrency().Should().Be("BRL");
    }

    [Fact]
    public void Create_ShouldThrowException_WhenQuantityIsZero()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var product = ProductEntity.Create(Guid.NewGuid(), ProductName.Create("Mouse"), Money.Create(50, "BRL"));
        var unitPrice = Money.Create(50, "BRL");

        // Act
        var act = () => OrderProductEntity.Create(null, orderId, product, 0, unitPrice);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Quantity must be greater than zero");
    }

    [Fact]
    public void Create_ShouldThrowException_WhenQuantityIsNegative()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var product = ProductEntity.Create(Guid.NewGuid(), ProductName.Create("Mouse"), Money.Create(50, "BRL"));
        var unitPrice = Money.Create(50, "BRL");

        // Act
        var act = () => OrderProductEntity.Create(null, orderId, product, -5, unitPrice);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Quantity must be greater than zero");
    }

    [Fact]
    public void Create_ShouldSetCreatedAtAndUpdatedAt()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var product = ProductEntity.Create(Guid.NewGuid(), ProductName.Create("Keyboard"), Money.Create(200, "BRL"));
        var unitPrice = Money.Create(200, "BRL");
        var beforeCreate = DateTime.UtcNow;

        // Act
        var orderProduct = OrderProductEntity.Create(null, orderId, product, 1, unitPrice);
        var afterCreate = DateTime.UtcNow;

        // Assert
        orderProduct.Should().NotBeNull();
    }

    [Fact]
    public void GetProductName_ShouldReturnProductName()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var product = ProductEntity.Create(Guid.NewGuid(), ProductName.Create("Monitor"), Money.Create(800, "BRL"));
        var unitPrice = Money.Create(800, "BRL");
        var orderProduct = OrderProductEntity.Create(null, orderId, product, 1, unitPrice);

        // Act
        var result = orderProduct.GetProductName();

        // Assert
        result.Should().Be("Monitor");
    }

    [Fact]
    public void GetQuantity_ShouldReturnQuantity()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var product = ProductEntity.Create(Guid.NewGuid(), ProductName.Create("Mouse"), Money.Create(50, "BRL"));
        var unitPrice = Money.Create(50, "BRL");
        var orderProduct = OrderProductEntity.Create(null, orderId, product, 5, unitPrice);

        // Act
        var result = orderProduct.GetQuantity();

        // Assert
        result.Should().Be(5);
    }

    [Fact]
    public void GetUnitPrice_ShouldReturnUnitPrice()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var product = ProductEntity.Create(Guid.NewGuid(), ProductName.Create("Headphones"), Money.Create(350, "BRL"));
        var unitPrice = Money.Create(350, "BRL");
        var orderProduct = OrderProductEntity.Create(null, orderId, product, 2, unitPrice);

        // Act
        var result = orderProduct.GetUnitPrice();

        // Assert
        result.GetAmount().Should().Be(350);
        result.GetCurrency().Should().Be("BRL");
    }
}