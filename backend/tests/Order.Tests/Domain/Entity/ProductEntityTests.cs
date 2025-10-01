using FluentAssertions;
using Order.Domain.Entity;
using Order.Domain.ValueObject;

namespace Order.Tests.Domain.Entity;

public class ProductEntityTests
{
    [Fact]
    public void Create_ShouldCreateProduct_WhenValidParameters()
    {
        // Arrange
        var id = Guid.NewGuid();
        var name = ProductName.Create("Laptop");
        var value = Money.Create(1500, "BRL");

        // Act
        var product = ProductEntity.Create(id, name, value);

        // Assert
        product.Should().NotBeNull();
        product.GetId().Should().Be(id);
        product.GetName().Should().Be("Laptop");
        product.GetValue().GetAmount().Should().Be(1500);
        product.GetValue().GetCurrency().Should().Be("BRL");
    }

    [Fact]
    public void Create_ShouldCreateProduct_WithNullId()
    {
        // Arrange
        var name = ProductName.Create("Mouse");
        var value = Money.Create(50, "USD");

        // Act
        var product = ProductEntity.Create(null, name, value);

        // Assert
        product.Should().NotBeNull();
        product.GetId().Should().BeNull();
    }


    [Fact]
    public void GetName_ShouldReturnProductName()
    {
        // Arrange
        var name = ProductName.Create("Monitor");
        var value = Money.Create(800, "BRL");
        var product = ProductEntity.Create(Guid.NewGuid(), name, value);

        // Act
        var result = product.GetName();

        // Assert
        result.Should().Be("Monitor");
    }

    [Fact]
    public void GetValue_ShouldReturnMoneyValue()
    {
        // Arrange
        var name = ProductName.Create("Headphones");
        var value = Money.Create(350.50m, "BRL");
        var product = ProductEntity.Create(Guid.NewGuid(), name, value);

        // Act
        var result = product.GetValue();

        // Assert
        result.GetAmount().Should().Be(350.50m);
        result.GetCurrency().Should().Be("BRL");
    }
}