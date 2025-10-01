using FluentAssertions;
using Order.Domain.ValueObject;

namespace Order.Tests.Domain.ValueObject;

public class ProductNameTests
{
    [Theory]
    [InlineData("Laptop")]
    [InlineData("Gaming Mouse")]
    [InlineData("Ultra HD Monitor 27 inch")]
    public void Create_ShouldCreateProductName_WhenValidName(string productName)
    {
        // Act
        var name = ProductName.Create(productName);

        // Assert
        name.Should().NotBeNull();
        name.ToString().Should().Be(productName);
    }

    [Fact]
    public void Create_ShouldThrowException_WhenNameIsEmpty()
    {
        // Act
        var act = () => ProductName.Create("");

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Product name cannot be empty*");
    }

    [Fact]
    public void Create_ShouldThrowException_WhenNameIsNull()
    {
        // Act
        var act = () => ProductName.Create(null!);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Product name cannot be empty*");
    }

    [Fact]
    public void Create_ShouldThrowException_WhenNameIsTooShort()
    {
        // Act
        var act = () => ProductName.Create("AB");

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Product name must be at least 3 characters*");
    }

    [Fact]
    public void Create_ShouldThrowException_WhenNameExceedsMaxLength()
    {
        // Arrange
        var longName = new string('a', 101);

        // Act
        var act = () => ProductName.Create(longName);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Product name cannot exceed 100 characters*");
    }

    [Fact]
    public void Create_ShouldTrimWhitespace()
    {
        // Act
        var name = ProductName.Create("  Laptop  ");

        // Assert
        name.ToString().Should().Be("Laptop");
    }

    [Fact]
    public void ImplicitConversion_FromString_ShouldWork()
    {
        // Act
        ProductName name = "Gaming Mouse";

        // Assert
        name.Should().NotBeNull();
        name.ToString().Should().Be("Gaming Mouse");
    }

    [Fact]
    public void ImplicitConversion_ToString_ShouldWork()
    {
        // Arrange
        var productName = ProductName.Create("Laptop");

        // Act
        string name = productName;

        // Assert
        name.Should().Be("Laptop");
    }

    [Fact]
    public void Equals_ShouldReturnTrue_WhenSameName()
    {
        // Arrange
        var name1 = ProductName.Create("Laptop");
        var name2 = ProductName.Create("Laptop");

        // Act & Assert
        name1.Equals(name2).Should().BeTrue();
        (name1 == name2).Should().BeTrue();
    }

    [Fact]
    public void Equals_ShouldReturnFalse_WhenDifferentName()
    {
        // Arrange
        var name1 = ProductName.Create("Laptop");
        var name2 = ProductName.Create("Mouse");

        // Act & Assert
        name1.Equals(name2).Should().BeFalse();
        (name1 != name2).Should().BeTrue();
    }

    [Fact]
    public void GetHashCode_ShouldBeSame_ForEqualNames()
    {
        // Arrange
        var name1 = ProductName.Create("Laptop");
        var name2 = ProductName.Create("Laptop");

        // Act & Assert
        name1.GetHashCode().Should().Be(name2.GetHashCode());
    }

    [Fact]
    public void ToString_ShouldReturnValue()
    {
        // Arrange
        var name = ProductName.Create("Gaming Mouse");

        // Act
        var result = name.ToString();

        // Assert
        result.Should().Be("Gaming Mouse");
    }
}