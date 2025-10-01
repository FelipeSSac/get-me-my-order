using FluentAssertions;
using Order.Domain.ValueObject;

namespace Order.Tests.Domain.ValueObject;

public class MoneyTests
{
    [Fact]
    public void Create_ShouldCreateMoney_WhenValidParameters()
    {
        // Act
        var money = Money.Create(100.50m, "BRL");

        // Assert
        money.Should().NotBeNull();
        money.GetAmount().Should().Be(100.50m);
        money.GetCurrency().Should().Be("BRL");
    }

    [Fact]
    public void Create_ShouldUseDefaultCurrency_WhenCurrencyNotProvided()
    {
        // Act
        var money = Money.Create(100);

        // Assert
        money.GetCurrency().Should().Be("BRL");
    }

    [Fact]
    public void Create_ShouldThrowException_WhenAmountIsNegative()
    {
        // Act
        var act = () => Money.Create(-10, "BRL");

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Amount cannot be negative*");
    }

    [Fact]
    public void Create_ShouldThrowException_WhenCurrencyIsEmpty()
    {
        // Act
        var act = () => Money.Create(100, "");

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Currency cannot be empty*");
    }

    [Fact]
    public void Create_ShouldThrowException_WhenCurrencyIsNotThreeLetters()
    {
        // Act
        var act = () => Money.Create(100, "US");

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Currency must be a 3-letter ISO code*");
    }

    [Fact]
    public void Create_ShouldConvertCurrencyToUpperCase()
    {
        // Act
        var money = Money.Create(100, "usd");

        // Assert
        money.GetCurrency().Should().Be("USD");
    }

    [Fact]
    public void Zero_ShouldCreateMoneyWithZeroAmount()
    {
        // Act
        var money = Money.Zero("EUR");

        // Assert
        money.GetAmount().Should().Be(0);
        money.GetCurrency().Should().Be("EUR");
    }

    [Fact]
    public void Add_ShouldAddTwoMoneyValues_WhenSameCurrency()
    {
        // Arrange
        var money1 = Money.Create(100, "BRL");
        var money2 = Money.Create(50, "BRL");

        // Act
        var result = money1.Add(money2);

        // Assert
        result.GetAmount().Should().Be(150);
        result.GetCurrency().Should().Be("BRL");
    }

    [Fact]
    public void Add_ShouldThrowException_WhenDifferentCurrencies()
    {
        // Arrange
        var money1 = Money.Create(100, "BRL");
        var money2 = Money.Create(50, "USD");

        // Act
        var act = () => money1.Add(money2);

        // Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Cannot add different currencies: BRL and USD");
    }

    [Fact]
    public void Subtract_ShouldSubtractTwoMoneyValues_WhenSameCurrency()
    {
        // Arrange
        var money1 = Money.Create(100, "BRL");
        var money2 = Money.Create(30, "BRL");

        // Act
        var result = money1.Subtract(money2);

        // Assert
        result.GetAmount().Should().Be(70);
        result.GetCurrency().Should().Be("BRL");
    }

    [Fact]
    public void Subtract_ShouldThrowException_WhenDifferentCurrencies()
    {
        // Arrange
        var money1 = Money.Create(100, "BRL");
        var money2 = Money.Create(30, "USD");

        // Act
        var act = () => money1.Subtract(money2);

        // Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Cannot subtract different currencies: BRL and USD");
    }

    [Fact]
    public void Multiply_ShouldMultiplyMoneyByMultiplier()
    {
        // Arrange
        var money = Money.Create(50, "BRL");

        // Act
        var result = money.Multiply(3);

        // Assert
        result.GetAmount().Should().Be(150);
        result.GetCurrency().Should().Be("BRL");
    }

    [Fact]
    public void Equals_ShouldReturnTrue_WhenSameAmountAndCurrency()
    {
        // Arrange
        var money1 = Money.Create(100, "BRL");
        var money2 = Money.Create(100, "BRL");

        // Act & Assert
        money1.Equals(money2).Should().BeTrue();
        (money1 == money2).Should().BeTrue();
    }

    [Fact]
    public void Equals_ShouldReturnFalse_WhenDifferentAmount()
    {
        // Arrange
        var money1 = Money.Create(100, "BRL");
        var money2 = Money.Create(50, "BRL");

        // Act & Assert
        money1.Equals(money2).Should().BeFalse();
        (money1 != money2).Should().BeTrue();
    }

    [Fact]
    public void Equals_ShouldReturnFalse_WhenDifferentCurrency()
    {
        // Arrange
        var money1 = Money.Create(100, "BRL");
        var money2 = Money.Create(100, "USD");

        // Act & Assert
        money1.Equals(money2).Should().BeFalse();
    }

    [Fact]
    public void GreaterThan_ShouldReturnTrue_WhenFirstIsGreater()
    {
        // Arrange
        var money1 = Money.Create(100, "BRL");
        var money2 = Money.Create(50, "BRL");

        // Act & Assert
        (money1 > money2).Should().BeTrue();
        (money1 >= money2).Should().BeTrue();
    }

    [Fact]
    public void LessThan_ShouldReturnTrue_WhenFirstIsLess()
    {
        // Arrange
        var money1 = Money.Create(50, "BRL");
        var money2 = Money.Create(100, "BRL");

        // Act & Assert
        (money1 < money2).Should().BeTrue();
        (money1 <= money2).Should().BeTrue();
    }

    [Fact]
    public void Comparison_ShouldThrowException_WhenDifferentCurrencies()
    {
        // Arrange
        var money1 = Money.Create(100, "BRL");
        var money2 = Money.Create(50, "USD");

        // Act
        var act = () => money1 > money2;

        // Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Cannot compare different currencies: BRL and USD");
    }

    [Fact]
    public void ToString_ShouldReturnFormattedString()
    {
        // Arrange
        var money = Money.Create(1234.56m, "BRL");

        // Act
        var result = money.ToString();

        // Assert
        result.Should().Be("1,234.56 BRL");
    }
}