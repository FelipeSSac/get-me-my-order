using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Order.Application.UseCase;
using Order.Domain.Entity;
using Order.Domain.Enum;
using Order.Domain.Repository;
using Order.Domain.ValueObject;

namespace Order.Tests.Application.UseCase;

public class GetOrderUseCaseTests
{
    private readonly Mock<IOrderRepository> _orderRepositoryMock;
    private readonly Mock<ILogger<GetOrderUseCase>> _loggerMock;
    private readonly GetOrderUseCase _sut;

    public GetOrderUseCaseTests()
    {
        _orderRepositoryMock = new Mock<IOrderRepository>();
        _loggerMock = new Mock<ILogger<GetOrderUseCase>>();

        _sut = new GetOrderUseCase(
            _orderRepositoryMock.Object,
            _loggerMock.Object
        );
    }

    [Fact]
    public async Task Execute_ShouldReturnOrder_WhenOrderExists()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var client = ClientEntity.Create(
            Guid.NewGuid(),
            PersonName.Create("Emma", "Davis"),
            Email.Create("emma.davis@example.com")
        );

        var product = ProductEntity.Create(
            Guid.NewGuid(),
            ProductName.Create("Monitor"),
            Money.Create(500m, "BRL")
        );

        var orderProducts = new List<OrderProductEntity>
        {
            OrderProductEntity.Create(null, Guid.NewGuid(), product, 1, Money.Create(500m, "BRL"))
        };

        var order = OrderEntity.Create(orderId, client, orderProducts, OrderStatus.Pending);

        _orderRepositoryMock
            .Setup(x => x.GetByIdAsync(orderId, default))
            .ReturnsAsync(order);

        // Act
        var result = await _sut.Execute(orderId.ToString());

        // Assert
        result.Should().NotBeNull();
        result!.GetId().Should().Be(orderId);
        result.GetStatus().Should().Be(OrderStatus.Pending);
        result.GetOrderProducts().Should().HaveCount(1);

        _orderRepositoryMock.Verify(x => x.GetByIdAsync(orderId, default), Times.Once);
    }

    [Fact]
    public async Task Execute_ShouldReturnNull_WhenOrderDoesNotExist()
    {
        // Arrange
        var orderId = Guid.NewGuid();

        _orderRepositoryMock
            .Setup(x => x.GetByIdAsync(orderId, default))
            .ReturnsAsync((OrderEntity?)null);

        // Act
        var result = await _sut.Execute(orderId.ToString());

        // Assert
        result.Should().BeNull();

        _orderRepositoryMock.Verify(x => x.GetByIdAsync(orderId, default), Times.Once);
    }

    [Fact]
    public async Task Execute_ShouldThrowException_WhenIdIsInvalid()
    {
        // Arrange
        var invalidId = "invalid-guid";

        // Act
        var act = async () => await _sut.Execute(invalidId);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("Invalid order id");

        _orderRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<Guid>(), default), Times.Never);
    }

    [Fact]
    public async Task Execute_ShouldThrowException_WhenIdIsEmpty()
    {
        // Arrange
        var emptyId = "";

        // Act
        var act = async () => await _sut.Execute(emptyId);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>();

        _orderRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<Guid>(), default), Times.Never);
    }

    [Fact]
    public async Task Execute_ShouldLogWarning_WhenOrderNotFound()
    {
        // Arrange
        var orderId = Guid.NewGuid();

        _orderRepositoryMock
            .Setup(x => x.GetByIdAsync(orderId, default))
            .ReturnsAsync((OrderEntity?)null);

        // Act
        await _sut.Execute(orderId.ToString());

        // Assert
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("not found")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task Execute_ShouldLogInformation_WhenOrderFound()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var client = ClientEntity.Create(
            Guid.NewGuid(),
            PersonName.Create("Frank", "Miller"),
            Email.Create("frank.miller@example.com")
        );

        var product = ProductEntity.Create(
            Guid.NewGuid(),
            ProductName.Create("Keyboard"),
            Money.Create(150m, "BRL")
        );

        var orderProducts = new List<OrderProductEntity>
        {
            OrderProductEntity.Create(null, Guid.NewGuid(), product, 2, Money.Create(150m, "BRL"))
        };

        var order = OrderEntity.Create(orderId, client, orderProducts, OrderStatus.Processing);

        _orderRepositoryMock
            .Setup(x => x.GetByIdAsync(orderId, default))
            .ReturnsAsync(order);

        // Act
        await _sut.Execute(orderId.ToString());

        // Assert
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("found with status")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.AtLeastOnce);
    }
}
