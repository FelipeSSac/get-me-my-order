using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Order.Application.Service;
using Order.Application.UseCase;
using Order.Domain.Entity;
using Order.Domain.Enum;
using Order.Domain.Repository;
using Order.Domain.ValueObject;

namespace Order.Tests.Application.UseCase;

public class ProcessOrderUseCaseTests
{
    private readonly Mock<IOrderRepository> _orderRepositoryMock;
    private readonly Mock<IOrderNotificationService> _notificationServiceMock;
    private readonly Mock<IDomainEventPublisherService> _eventPublisherServiceMock;
    private readonly Mock<ILogger<ProcessOrderUseCase>> _loggerMock;
    private readonly ProcessOrderUseCase _sut;

    public ProcessOrderUseCaseTests()
    {
        _orderRepositoryMock = new Mock<IOrderRepository>();
        _notificationServiceMock = new Mock<IOrderNotificationService>();
        _eventPublisherServiceMock = new Mock<IDomainEventPublisherService>();
        _loggerMock = new Mock<ILogger<ProcessOrderUseCase>>();

        _sut = new ProcessOrderUseCase(
            _orderRepositoryMock.Object,
            _notificationServiceMock.Object,
            _eventPublisherServiceMock.Object,
            _loggerMock.Object
        );
    }

    [Fact]
    public async Task Execute_ShouldProcessOrder_WhenOrderExists()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var order = CreateTestOrder(orderId, OrderStatus.Pending);

        _orderRepositoryMock
            .Setup(x => x.GetByIdAsync(orderId, default))
            .ReturnsAsync(order);

        _orderRepositoryMock
            .Setup(x => x.UpdateAsync(It.IsAny<OrderEntity>(), default))
            .Returns(Task.CompletedTask);

        _notificationServiceMock
            .Setup(x => x.NotifyOrderStatusChanged(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<object>()))
            .Returns(Task.CompletedTask);

        _eventPublisherServiceMock
            .Setup(x => x.PublishAsync(It.IsAny<object>()))
            .Returns(Task.CompletedTask);

        // Act
        await _sut.Execute(orderId.ToString());

        // Assert
        _orderRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<OrderEntity>(), default), Times.Exactly(2));
        _notificationServiceMock.Verify(
            x => x.NotifyOrderStatusChanged(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<object>()),
            Times.Exactly(2)
        );
        _eventPublisherServiceMock.Verify(x => x.PublishAsync(It.IsAny<object>()), Times.Exactly(2));
    }

    [Fact]
    public async Task Execute_ShouldThrowException_WhenOrderNotFound()
    {
        // Arrange
        var orderId = Guid.NewGuid();

        _orderRepositoryMock
            .Setup(x => x.GetByIdAsync(orderId, default))
            .ReturnsAsync((OrderEntity?)null);

        // Act
        var act = async () => await _sut.Execute(orderId.ToString());

        // Assert
        await act.Should().ThrowAsync<Exception>()
            .WithMessage("Order not found!");

        _orderRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<OrderEntity>(), default), Times.Never);
    }

    [Fact]
    public async Task Execute_ShouldUpdateOrderToProcessing_ThenDone()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var order = CreateTestOrder(orderId, OrderStatus.Pending);
        var capturedOrders = new List<OrderEntity>();

        _orderRepositoryMock
            .Setup(x => x.GetByIdAsync(orderId, default))
            .ReturnsAsync(order);

        _orderRepositoryMock
            .Setup(x => x.UpdateAsync(It.IsAny<OrderEntity>(), default))
            .Callback<OrderEntity, CancellationToken>((o, ct) => capturedOrders.Add(o))
            .Returns(Task.CompletedTask);

        _notificationServiceMock
            .Setup(x => x.NotifyOrderStatusChanged(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<object>()))
            .Returns(Task.CompletedTask);

        _eventPublisherServiceMock
            .Setup(x => x.PublishAsync(It.IsAny<object>()))
            .Returns(Task.CompletedTask);

        // Act
        await _sut.Execute(orderId.ToString());

        // Assert
        capturedOrders.Should().HaveCount(2);
        capturedOrders[0].GetStatus().Should().Be(OrderStatus.Processing);
        capturedOrders[1].GetStatus().Should().Be(OrderStatus.Done);
    }

    [Fact]
    public async Task Execute_ShouldSendNotifications_ForBothStatusChanges()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var order = CreateTestOrder(orderId, OrderStatus.Pending);
        var capturedStatuses = new List<string>();

        _orderRepositoryMock
            .Setup(x => x.GetByIdAsync(orderId, default))
            .ReturnsAsync(order);

        _orderRepositoryMock
            .Setup(x => x.UpdateAsync(It.IsAny<OrderEntity>(), default))
            .Returns(Task.CompletedTask);

        _notificationServiceMock
            .Setup(x => x.NotifyOrderStatusChanged(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<object>()))
            .Callback<string, string, object?>((id, status, data) => capturedStatuses.Add(status))
            .Returns(Task.CompletedTask);

        _eventPublisherServiceMock
            .Setup(x => x.PublishAsync(It.IsAny<object>()))
            .Returns(Task.CompletedTask);

        // Act
        await _sut.Execute(orderId.ToString());

        // Assert
        capturedStatuses.Should().HaveCount(2);
        capturedStatuses[0].Should().Be(OrderStatus.Processing.ToString());
        capturedStatuses[1].Should().Be(OrderStatus.Done.ToString());
    }

    [Fact]
    public async Task Execute_ShouldLogInformation_WhenProcessingStarts()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var order = CreateTestOrder(orderId, OrderStatus.Pending);

        _orderRepositoryMock
            .Setup(x => x.GetByIdAsync(orderId, default))
            .ReturnsAsync(order);

        _orderRepositoryMock
            .Setup(x => x.UpdateAsync(It.IsAny<OrderEntity>(), default))
            .Returns(Task.CompletedTask);

        _notificationServiceMock
            .Setup(x => x.NotifyOrderStatusChanged(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<object>()))
            .Returns(Task.CompletedTask);

        _eventPublisherServiceMock
            .Setup(x => x.PublishAsync(It.IsAny<object>()))
            .Returns(Task.CompletedTask);

        // Act
        await _sut.Execute(orderId.ToString());

        // Assert
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Starting to process order")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task Execute_ShouldLogInformation_WhenProcessingCompletes()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var order = CreateTestOrder(orderId, OrderStatus.Pending);

        _orderRepositoryMock
            .Setup(x => x.GetByIdAsync(orderId, default))
            .ReturnsAsync(order);

        _orderRepositoryMock
            .Setup(x => x.UpdateAsync(It.IsAny<OrderEntity>(), default))
            .Returns(Task.CompletedTask);

        _notificationServiceMock
            .Setup(x => x.NotifyOrderStatusChanged(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<object>()))
            .Returns(Task.CompletedTask);

        _eventPublisherServiceMock
            .Setup(x => x.PublishAsync(It.IsAny<object>()))
            .Returns(Task.CompletedTask);

        // Act
        await _sut.Execute(orderId.ToString());

        // Assert
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("processing completed successfully")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
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
        try
        {
            await _sut.Execute(orderId.ToString());
        }
        catch
        {
            // Expected exception
        }

        // Assert
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("not found for processing")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task Execute_ShouldIncludeOrderData_InNotification()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var order = CreateTestOrder(orderId, OrderStatus.Pending);
        object? capturedData = null;

        _orderRepositoryMock
            .Setup(x => x.GetByIdAsync(orderId, default))
            .ReturnsAsync(order);

        _orderRepositoryMock
            .Setup(x => x.UpdateAsync(It.IsAny<OrderEntity>(), default))
            .Returns(Task.CompletedTask);

        _notificationServiceMock
            .Setup(x => x.NotifyOrderStatusChanged(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<object>()))
            .Callback<string, string, object?>((id, status, data) => capturedData = data)
            .Returns(Task.CompletedTask);

        _eventPublisherServiceMock
            .Setup(x => x.PublishAsync(It.IsAny<object>()))
            .Returns(Task.CompletedTask);

        // Act
        await _sut.Execute(orderId.ToString());

        // Assert
        capturedData.Should().NotBeNull();
    }

    private OrderEntity CreateTestOrder(Guid orderId, OrderStatus status)
    {
        var client = ClientEntity.Create(
            Guid.NewGuid(),
            PersonName.Create("Grace", "Wilson"),
            Email.Create("grace.wilson@example.com")
        );

        var product = ProductEntity.Create(
            Guid.NewGuid(),
            ProductName.Create("Headphones"),
            Money.Create(200m, "BRL")
        );

        var orderProducts = new List<OrderProductEntity>
        {
            OrderProductEntity.Create(
                null,
                Guid.NewGuid(),
                product,
                1,
                Money.Create(200m, "BRL")
            )
        };

        return OrderEntity.Create(orderId, client, orderProducts, status);
    }
}
