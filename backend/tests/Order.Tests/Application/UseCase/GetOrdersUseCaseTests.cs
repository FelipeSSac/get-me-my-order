using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Order.Application.UseCase;
using Order.Domain.Entity;
using Order.Domain.Enum;
using Order.Domain.Repository;
using Order.Domain.ValueObject;

namespace Order.Tests.Application.UseCase;

public class GetOrdersUseCaseTests
{
    private readonly Mock<IOrderRepository> _orderRepositoryMock;
    private readonly Mock<ILogger<GetOrdersUseCase>> _loggerMock;
    private readonly GetOrdersUseCase _sut;

    public GetOrdersUseCaseTests()
    {
        _orderRepositoryMock = new Mock<IOrderRepository>();
        _loggerMock = new Mock<ILogger<GetOrdersUseCase>>();

        _sut = new GetOrdersUseCase(
            _orderRepositoryMock.Object,
            _loggerMock.Object
        );
    }

    [Fact]
    public async Task Execute_ShouldReturnPaginatedOrders_WhenValidRequest()
    {
        // Arrange
        var orders = CreateOrderList(5);

        _orderRepositoryMock
            .Setup(x => x.GetPaginatedAsync(1, 10, null, default))
            .ReturnsAsync((orders, 5));

        // Act
        var result = await _sut.Execute(1, 10);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(5);
        result.Page.Should().Be(1);
        result.PageSize.Should().Be(10);
        result.TotalItems.Should().Be(5);

        _orderRepositoryMock.Verify(x => x.GetPaginatedAsync(1, 10, null, default), Times.Once);
    }

    [Fact]
    public async Task Execute_ShouldReturnFilteredOrders_WhenStatusProvided()
    {
        // Arrange
        var orders = CreateOrderList(3, OrderStatus.Processing);

        _orderRepositoryMock
            .Setup(x => x.GetPaginatedAsync(1, 10, OrderStatus.Processing, default))
            .ReturnsAsync((orders, 3));

        // Act
        var result = await _sut.Execute(1, 10, OrderStatus.Processing);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(3);
        result.Items.Should().AllSatisfy(o => o.GetStatus().Should().Be(OrderStatus.Processing));

        _orderRepositoryMock.Verify(x => x.GetPaginatedAsync(1, 10, OrderStatus.Processing, default), Times.Once);
    }

    [Fact]
    public async Task Execute_ShouldThrowException_WhenPageIsZero()
    {
        // Arrange & Act
        var act = async () => await _sut.Execute(0, 10);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("Page must be greater than 0*");

        _orderRepositoryMock.Verify(x => x.GetPaginatedAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<OrderStatus?>(), default), Times.Never);
    }

    [Fact]
    public async Task Execute_ShouldThrowException_WhenPageIsNegative()
    {
        // Arrange & Act
        var act = async () => await _sut.Execute(-1, 10);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("Page must be greater than 0*");
    }

    [Fact]
    public async Task Execute_ShouldThrowException_WhenPageSizeIsZero()
    {
        // Arrange & Act
        var act = async () => await _sut.Execute(1, 0);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("PageSize must be between 1 and 100*");
    }

    [Fact]
    public async Task Execute_ShouldThrowException_WhenPageSizeExceeds100()
    {
        // Arrange & Act
        var act = async () => await _sut.Execute(1, 101);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("PageSize must be between 1 and 100*");
    }

    [Fact]
    public async Task Execute_ShouldReturnEmptyList_WhenNoOrdersExist()
    {
        // Arrange
        _orderRepositoryMock
            .Setup(x => x.GetPaginatedAsync(1, 10, null, default))
            .ReturnsAsync((new List<OrderEntity>(), 0));

        // Act
        var result = await _sut.Execute(1, 10);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().BeEmpty();
        result.TotalItems.Should().Be(0);
    }

    [Fact]
    public async Task Execute_ShouldReturnCorrectTotalPages()
    {
        // Arrange
        var orders = CreateOrderList(10);

        _orderRepositoryMock
            .Setup(x => x.GetPaginatedAsync(1, 5, null, default))
            .ReturnsAsync((orders.Take(5), 10));

        // Act
        var result = await _sut.Execute(1, 5);

        // Assert
        result.Should().NotBeNull();
        result.TotalPages.Should().Be(2); // 10 total / 5 per page = 2 pages
    }

    [Fact]
    public async Task Execute_ShouldLogInformation_WhenOrdersRetrieved()
    {
        // Arrange
        var orders = CreateOrderList(3);

        _orderRepositoryMock
            .Setup(x => x.GetPaginatedAsync(1, 10, null, default))
            .ReturnsAsync((orders, 3));

        // Act
        await _sut.Execute(1, 10);

        // Assert
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Retrieved")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.AtLeastOnce);
    }

    [Fact]
    public async Task Execute_ShouldReturnOrdersWithDifferentStatuses()
    {
        // Arrange
        var pendingOrders = CreateOrderList(2, OrderStatus.Pending);

        _orderRepositoryMock
            .Setup(x => x.GetPaginatedAsync(1, 10, OrderStatus.Pending, default))
            .ReturnsAsync((pendingOrders, 2));

        // Act
        var result = await _sut.Execute(1, 10, OrderStatus.Pending);

        // Assert
        result.Items.Should().AllSatisfy(o => o.GetStatus().Should().Be(OrderStatus.Pending));
    }

    private List<OrderEntity> CreateOrderList(int count, OrderStatus status = OrderStatus.Pending)
    {
        var orders = new List<OrderEntity>();

        for (int i = 0; i < count; i++)
        {
            var client = ClientEntity.Create(
                Guid.NewGuid(),
                PersonName.Create($"Client{i}", $"User{i}"),
                Email.Create($"client{i}@example.com")
            );

            var product = ProductEntity.Create(
                Guid.NewGuid(),
                ProductName.Create($"Product {i}"),
                Money.Create(100m * (i + 1), "BRL")
            );

            var orderProducts = new List<OrderProductEntity>
            {
                OrderProductEntity.Create(
                    null,
                    Guid.NewGuid(),
                    product,
                    1,
                    Money.Create(100m * (i + 1), "BRL")
                )
            };

            var order = OrderEntity.Create(
                Guid.NewGuid(),
                client,
                orderProducts,
                status
            );

            orders.Add(order);
        }

        return orders;
    }
}
