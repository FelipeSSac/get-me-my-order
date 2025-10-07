using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Order.Application.UseCase;
using Order.Domain.Entity;
using Order.Domain.Repository;
using Order.Infrastructure.Api.Controller.Request;

namespace Order.Tests.Application.UseCase;

public class CreateProductUseCaseTests
{
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly Mock<ILogger<CreateProductUseCase>> _loggerMock;
    private readonly CreateProductUseCase _sut;

    public CreateProductUseCaseTests()
    {
        _productRepositoryMock = new Mock<IProductRepository>();
        _loggerMock = new Mock<ILogger<CreateProductUseCase>>();

        _sut = new CreateProductUseCase(
            _productRepositoryMock.Object,
            _loggerMock.Object
        );
    }

    [Fact]
    public async Task Execute_ShouldCreateProduct_WhenValidRequest()
    {
        // Arrange
        var request = new CreateProductRequest(
            "Laptop",
            2500.00m,
            "BRL"
        );

        _productRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<ProductEntity>(), default))
            .ReturnsAsync((ProductEntity product, CancellationToken _) => product);

        // Act
        var result = await _sut.Execute(request);

        // Assert
        result.Should().NotBeNull();
        result.GetName().Should().Be("Laptop");
        result.GetValue().GetAmount().Should().Be(2500.00m);
        result.GetValue().GetCurrency().Should().Be("BRL");

        _productRepositoryMock.Verify(x => x.AddAsync(It.IsAny<ProductEntity>(), default), Times.Once);
    }

    [Fact]
    public async Task Execute_ShouldCreateProduct_WithDifferentCurrency()
    {
        // Arrange
        var request = new CreateProductRequest(
            "Smartphone",
            800.00m,
            "USD"
        );

        _productRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<ProductEntity>(), default))
            .ReturnsAsync((ProductEntity product, CancellationToken _) => product);

        // Act
        var result = await _sut.Execute(request);

        // Assert
        result.Should().NotBeNull();
        result.GetName().Should().Be("Smartphone");
        result.GetValue().GetAmount().Should().Be(800.00m);
        result.GetValue().GetCurrency().Should().Be("USD");
    }

    [Fact]
    public async Task Execute_ShouldCreateProduct_WithZeroValue()
    {
        // Arrange
        var request = new CreateProductRequest(
            "Free Sample",
            0m,
            "BRL"
        );

        _productRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<ProductEntity>(), default))
            .ReturnsAsync((ProductEntity product, CancellationToken _) => product);

        // Act
        var result = await _sut.Execute(request);

        // Assert
        result.Should().NotBeNull();
        result.GetValue().GetAmount().Should().Be(0m);
    }

    [Fact]
    public async Task Execute_ShouldCreateProduct_WithLargeValue()
    {
        // Arrange
        var request = new CreateProductRequest(
            "Enterprise Server",
            50000.00m,
            "USD"
        );

        _productRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<ProductEntity>(), default))
            .ReturnsAsync((ProductEntity product, CancellationToken _) => product);

        // Act
        var result = await _sut.Execute(request);

        // Assert
        result.Should().NotBeNull();
        result.GetName().Should().Be("Enterprise Server");
        result.GetValue().GetAmount().Should().Be(50000.00m);
    }

    [Fact]
    public async Task Execute_ShouldLogInformation_WhenProductIsCreated()
    {
        // Arrange
        var request = new CreateProductRequest(
            "Tablet",
            1200.00m,
            "BRL"
        );

        _productRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<ProductEntity>(), default))
            .ReturnsAsync((ProductEntity product, CancellationToken _) => product);

        // Act
        await _sut.Execute(request);

        // Assert
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Creating product")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task Execute_ShouldLogError_WhenExceptionOccurs()
    {
        // Arrange
        var request = new CreateProductRequest(
            "Faulty Product",
            100.00m,
            "BRL"
        );

        _productRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<ProductEntity>(), default))
            .ThrowsAsync(new Exception("Database error"));

        // Act
        var act = async () => await _sut.Execute(request);

        // Assert
        await act.Should().ThrowAsync<Exception>();

        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Error creating product")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }
}
