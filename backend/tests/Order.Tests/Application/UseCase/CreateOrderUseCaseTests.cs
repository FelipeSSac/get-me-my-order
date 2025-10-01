using FluentAssertions;
using Moq;
using Order.Application.UseCase;
using Order.Domain.Entity;
using Order.Domain.Enum;
using Order.Domain.Repository;
using Order.Domain.ValueObject;
using Order.Infrastructure.Api.Controller.Request;
using Order.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Order.Tests.Application.UseCase;

public class CreateOrderUseCaseTests : IDisposable
{
    private readonly Mock<IOrderRepository> _orderRepositoryMock;
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly Mock<IClientRepository> _clientRepositoryMock;
    private readonly OrderDbContext _context;
    private readonly CreateOrderUseCase _sut;

    public CreateOrderUseCaseTests()
    {
        _orderRepositoryMock = new Mock<IOrderRepository>();
        _productRepositoryMock = new Mock<IProductRepository>();
        _clientRepositoryMock = new Mock<IClientRepository>();

        var options = new DbContextOptionsBuilder<OrderDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;
        _context = new OrderDbContext(options);

        _sut = new CreateOrderUseCase(
            _orderRepositoryMock.Object,
            _productRepositoryMock.Object,
            _clientRepositoryMock.Object,
            _context
        );
    }

    [Fact]
    public async Task Execute_ShouldCreateOrder_WhenValidRequest()
    {
        // Arrange
        var clientId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var client = ClientEntity.Create(clientId, PersonName.Create("John", "Doe"), Email.Create("john@example.com"));
        var product = ProductEntity.Create(productId, ProductName.Create("Test Product"), Money.Create(10, "BRL"));

        var request = new CreateOrderRequest(
            new List<CreateOrderProductRequest>
            {
                new CreateOrderProductRequest(productId, 2)
            },
            clientId
        );

        _clientRepositoryMock
            .Setup(x => x.GetByIdAsync(clientId, default))
            .ReturnsAsync(client);

        _productRepositoryMock
            .Setup(x => x.GetByIdAsync(productId, default))
            .ReturnsAsync(product);

        _orderRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<OrderEntity>(), default))
            .ReturnsAsync((OrderEntity order, CancellationToken _) => order);

        // Act
        var result = await _sut.Execute(request);

        // Assert
        result.Should().NotBeNull();
        result.GetClientId().Should().Be(clientId);
        result.GetOrderProducts().Should().HaveCount(1);
        result.GetOrderProducts()[0].GetQuantity().Should().Be(2);
        result.GetStatus().Should().Be(OrderStatus.Pending);
        result.GetTotalValue().GetAmount().Should().Be(20);
        result.GetTotalValue().GetCurrency().Should().Be("BRL");

        _orderRepositoryMock.Verify(x => x.AddAsync(It.IsAny<OrderEntity>(), default), Times.Once);
    }

    [Fact]
    public async Task Execute_ShouldThrowException_WhenClientDoesNotExist()
    {
        // Arrange
        var clientId = Guid.NewGuid();
        var productId = Guid.NewGuid();

        var request = new CreateOrderRequest(
            new List<CreateOrderProductRequest>
            {
                new CreateOrderProductRequest(productId, 2)
            },
            clientId
        );

        _clientRepositoryMock
            .Setup(x => x.GetByIdAsync(clientId, default))
            .ReturnsAsync((ClientEntity?)null);

        // Act
        var act = async () => await _sut.Execute(request);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("Client does not exists!");
    }

    [Fact]
    public async Task Execute_ShouldThrowException_WhenProductDoesNotExist()
    {
        // Arrange
        var clientId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var client = ClientEntity.Create(clientId, PersonName.Create("John", "Doe"), Email.Create("john@example.com"));

        var request = new CreateOrderRequest(
            new List<CreateOrderProductRequest>
            {
                new CreateOrderProductRequest(productId, 2)
            },
            clientId
        );

        _clientRepositoryMock
            .Setup(x => x.GetByIdAsync(clientId, default))
            .ReturnsAsync(client);

        _productRepositoryMock
            .Setup(x => x.GetByIdAsync(productId, default))
            .ReturnsAsync((ProductEntity?)null);

        // Act
        var act = async () => await _sut.Execute(request);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("Product does not exists!");
    }

    [Fact]
    public async Task Execute_ShouldCreateOrderWithMultipleProducts_WhenValidRequest()
    {
        // Arrange
        var clientId = Guid.NewGuid();
        var product1Id = Guid.NewGuid();
        var product2Id = Guid.NewGuid();

        var client = ClientEntity.Create(clientId, PersonName.Create("John", "Doe"), Email.Create("john@example.com"));
        var product1 = ProductEntity.Create(product1Id, ProductName.Create("Product 1"), Money.Create(10, "BRL"));
        var product2 = ProductEntity.Create(product2Id, ProductName.Create("Product 2"), Money.Create(15, "BRL"));

        var request = new CreateOrderRequest(
            new List<CreateOrderProductRequest>
            {
                new CreateOrderProductRequest(product1Id, 2),
                new CreateOrderProductRequest(product2Id, 1)
            },
            clientId
        );

        _clientRepositoryMock
            .Setup(x => x.GetByIdAsync(clientId, default))
            .ReturnsAsync(client);

        _productRepositoryMock
            .Setup(x => x.GetByIdAsync(product1Id, default))
            .ReturnsAsync(product1);

        _productRepositoryMock
            .Setup(x => x.GetByIdAsync(product2Id, default))
            .ReturnsAsync(product2);

        _orderRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<OrderEntity>(), default))
            .ReturnsAsync((OrderEntity order, CancellationToken _) => order);

        // Act
        var result = await _sut.Execute(request);

        // Assert
        result.Should().NotBeNull();
        result.GetOrderProducts().Should().HaveCount(2);
        result.GetTotalValue().GetAmount().Should().Be(35); // (10 * 2) + (15 * 1)
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}