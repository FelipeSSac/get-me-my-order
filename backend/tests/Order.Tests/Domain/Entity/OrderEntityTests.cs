using FluentAssertions;
using Order.Domain.Entity;
using Order.Domain.Enum;
using Order.Domain.ValueObject;

namespace Order.Tests.Domain.Entity;

public class OrderEntityTests
{
    [Fact]
    public void Create_ShouldCreateOrder_WhenValidParameters()
    {
        // Arrange
        var id = Guid.NewGuid();
        var client = ClientEntity.Create(Guid.NewGuid(), PersonName.Create("John", "Doe"), Email.Create("john@example.com"));
        var product = ProductEntity.Create(Guid.NewGuid(), ProductName.Create("Laptop"), Money.Create(1500, "BRL"));
        var orderProducts = new List<OrderProductEntity>
        {
            OrderProductEntity.Create(null, Guid.NewGuid(), product, 2, Money.Create(1500, "BRL"))
        };

        // Act
        var order = OrderEntity.Create(id, client, orderProducts, OrderStatus.Pending);

        // Assert
        order.Should().NotBeNull();
        order.GetId().Should().Be(id);
        order.GetOrderProducts().Should().HaveCount(1);
        order.GetStatus().Should().Be(OrderStatus.Pending);
        order.GetTotalValue().GetAmount().Should().Be(3000); // 1500 * 2
        order.GetTotalValue().GetCurrency().Should().Be("BRL");
    }

    [Fact]
    public void Create_ShouldThrowException_WhenOrderProductsIsEmpty()
    {
        // Arrange
        var client = ClientEntity.Create(Guid.NewGuid(), PersonName.Create("John", "Doe"), Email.Create("john@example.com"));
        var orderProducts = new List<OrderProductEntity>();

        // Act
        var act = () => OrderEntity.Create(Guid.NewGuid(), client, orderProducts, OrderStatus.Pending);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Order must have at least one product");
    }

    [Fact]
    public void Create_ShouldThrowException_WhenOrderProductsIsNull()
    {
        // Arrange
        var client = ClientEntity.Create(Guid.NewGuid(), PersonName.Create("John", "Doe"), Email.Create("john@example.com"));

        // Act
        var act = () => OrderEntity.Create(Guid.NewGuid(), client, null!, OrderStatus.Pending);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Order must have at least one product");
    }

    [Fact]
    public void Create_ShouldCalculateTotalValue_WithMultipleProducts()
    {
        // Arrange
        var client = ClientEntity.Create(Guid.NewGuid(), PersonName.Create("John", "Doe"), Email.Create("john@example.com"));
        var product1 = ProductEntity.Create(Guid.NewGuid(), ProductName.Create("Laptop"), Money.Create(1500, "BRL"));
        var product2 = ProductEntity.Create(Guid.NewGuid(), ProductName.Create("Mouse"), Money.Create(50, "BRL"));
        var orderProducts = new List<OrderProductEntity>
        {
            OrderProductEntity.Create(null, Guid.NewGuid(), product1, 2, Money.Create(1500, "BRL")),
            OrderProductEntity.Create(null, Guid.NewGuid(), product2, 3, Money.Create(50, "BRL"))
        };

        // Act
        var order = OrderEntity.Create(Guid.NewGuid(), client, orderProducts, OrderStatus.Pending);

        // Assert
        order.GetTotalValue().GetAmount().Should().Be(3150); // (1500 * 2) + (50 * 3)
    }

    [Fact]
    public void Create_ShouldThrowException_WhenProductsHaveDifferentCurrencies()
    {
        // Arrange
        var client = ClientEntity.Create(Guid.NewGuid(), PersonName.Create("John", "Doe"), Email.Create("john@example.com"));
        var product1 = ProductEntity.Create(Guid.NewGuid(), ProductName.Create("Laptop"), Money.Create(1500, "BRL"));
        var product2 = ProductEntity.Create(Guid.NewGuid(), ProductName.Create("Mouse"), Money.Create(50, "USD"));
        var orderProducts = new List<OrderProductEntity>
        {
            OrderProductEntity.Create(null, Guid.NewGuid(), product1, 1, Money.Create(1500, "BRL")),
            OrderProductEntity.Create(null, Guid.NewGuid(), product2, 1, Money.Create(50, "USD"))
        };

        // Act
        var act = () => OrderEntity.Create(Guid.NewGuid(), client, orderProducts, OrderStatus.Pending);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("All order products must have the same currency");
    }

    [Fact]
    public void ChangeStatus_ShouldChangeStatus_WhenValidTransition()
    {
        // Arrange
        var client = ClientEntity.Create(Guid.NewGuid(), PersonName.Create("John", "Doe"), Email.Create("john@example.com"));
        var product = ProductEntity.Create(Guid.NewGuid(), ProductName.Create("Laptop"), Money.Create(1500, "BRL"));
        var orderProducts = new List<OrderProductEntity>
        {
            OrderProductEntity.Create(null, Guid.NewGuid(), product, 1, Money.Create(1500, "BRL"))
        };
        var order = OrderEntity.Create(Guid.NewGuid(), client, orderProducts, OrderStatus.Pending);

        // Act
        var updatedOrder = order.ChangeStatus(OrderStatus.Processing);

        // Assert
        updatedOrder.GetStatus().Should().Be(OrderStatus.Processing);
    }

    [Fact]
    public void ChangeStatus_ShouldThrowException_WhenInvalidTransition()
    {
        // Arrange
        var client = ClientEntity.Create(Guid.NewGuid(), PersonName.Create("John", "Doe"), Email.Create("john@example.com"));
        var product = ProductEntity.Create(Guid.NewGuid(), ProductName.Create("Laptop"), Money.Create(1500, "BRL"));
        var orderProducts = new List<OrderProductEntity>
        {
            OrderProductEntity.Create(null, Guid.NewGuid(), product, 1, Money.Create(1500, "BRL"))
        };
        var order = OrderEntity.Create(Guid.NewGuid(), client, orderProducts, OrderStatus.Pending);

        // Act
        var act = () => order.ChangeStatus(OrderStatus.Done);

        // Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Cannot transition from Pending to Done. Status must progress sequentially.");
    }

    [Fact]
    public void ChangeStatus_ShouldUpdateUpdatedAt()
    {
        // Arrange
        var client = ClientEntity.Create(Guid.NewGuid(), PersonName.Create("John", "Doe"), Email.Create("john@example.com"));
        var product = ProductEntity.Create(Guid.NewGuid(), ProductName.Create("Laptop"), Money.Create(1500, "BRL"));
        var orderProducts = new List<OrderProductEntity>
        {
            OrderProductEntity.Create(null, Guid.NewGuid(), product, 1, Money.Create(1500, "BRL"))
        };
        var order = OrderEntity.Create(Guid.NewGuid(), client, orderProducts, OrderStatus.Pending);
        var originalUpdatedAt = order.GetUpdatedAt();

        // Wait a tiny bit to ensure time difference
        System.Threading.Thread.Sleep(10);

        // Act
        var updatedOrder = order.ChangeStatus(OrderStatus.Processing);

        // Assert
        updatedOrder.GetUpdatedAt().Should().BeAfter(originalUpdatedAt);
    }

    [Fact]
    public void GetOrderProducts_ShouldReturnAllProducts()
    {
        // Arrange
        var client = ClientEntity.Create(Guid.NewGuid(), PersonName.Create("John", "Doe"), Email.Create("john@example.com"));
        var product1 = ProductEntity.Create(Guid.NewGuid(), ProductName.Create("Laptop"), Money.Create(1500, "BRL"));
        var product2 = ProductEntity.Create(Guid.NewGuid(), ProductName.Create("Mouse"), Money.Create(50, "BRL"));
        var orderProducts = new List<OrderProductEntity>
        {
            OrderProductEntity.Create(null, Guid.NewGuid(), product1, 1, Money.Create(1500, "BRL")),
            OrderProductEntity.Create(null, Guid.NewGuid(), product2, 2, Money.Create(50, "BRL"))
        };
        var order = OrderEntity.Create(Guid.NewGuid(), client, orderProducts, OrderStatus.Pending);

        // Act
        var result = order.GetOrderProducts();

        // Assert
        result.Should().HaveCount(2);
        result[0].GetProductName().Should().Be("Laptop");
        result[1].GetProductName().Should().Be("Mouse");
    }

    [Fact]
    public void Create_ShouldSetCreatedAtAndUpdatedAt()
    {
        // Arrange
        var client = ClientEntity.Create(Guid.NewGuid(), PersonName.Create("John", "Doe"), Email.Create("john@example.com"));
        var product = ProductEntity.Create(Guid.NewGuid(), ProductName.Create("Laptop"), Money.Create(1500, "BRL"));
        var orderProducts = new List<OrderProductEntity>
        {
            OrderProductEntity.Create(null, Guid.NewGuid(), product, 1, Money.Create(1500, "BRL"))
        };
        var beforeCreate = DateTime.UtcNow;

        // Act
        var order = OrderEntity.Create(Guid.NewGuid(), client, orderProducts, OrderStatus.Pending);
        var afterCreate = DateTime.UtcNow;

        // Assert
        order.GetCreatedAt().Should().BeOnOrAfter(beforeCreate);
        order.GetCreatedAt().Should().BeOnOrBefore(afterCreate);
        order.GetUpdatedAt().Should().BeOnOrAfter(beforeCreate);
        order.GetUpdatedAt().Should().BeOnOrBefore(afterCreate);
    }
}