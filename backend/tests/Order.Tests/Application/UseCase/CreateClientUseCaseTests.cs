using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Order.Application.UseCase;
using Order.Domain.Entity;
using Order.Domain.Repository;
using Order.Domain.ValueObject;
using Order.Infrastructure.Api.Controller.Request;

namespace Order.Tests.Application.UseCase;

public class CreateClientUseCaseTests
{
    private readonly Mock<IClientRepository> _clientRepositoryMock;
    private readonly Mock<ILogger<CreateClientUseCase>> _loggerMock;
    private readonly CreateClientUseCase _sut;

    public CreateClientUseCaseTests()
    {
        _clientRepositoryMock = new Mock<IClientRepository>();
        _loggerMock = new Mock<ILogger<CreateClientUseCase>>();

        _sut = new CreateClientUseCase(
            _clientRepositoryMock.Object,
            _loggerMock.Object
        );
    }

    [Fact]
    public async Task Execute_ShouldCreateClient_WhenValidRequest()
    {
        // Arrange
        var request = new CreateClientRequest(
            "Alice",
            "Smith",
            "alice.smith@example.com"
        );

        _clientRepositoryMock
            .Setup(x => x.GetByEmailAsync(request.Email, default))
            .ReturnsAsync((ClientEntity?)null);

        _clientRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<ClientEntity>(), default))
            .ReturnsAsync((ClientEntity client, CancellationToken _) => client);

        // Act
        var result = await _sut.Execute(request);

        // Assert
        result.Should().NotBeNull();
        result.GetName().GetFirstName().Should().Be("Alice");
        result.GetName().GetLastName().Should().Be("Smith");
        result.GetEmail().ToString().Should().Be("alice.smith@example.com");

        _clientRepositoryMock.Verify(x => x.AddAsync(It.IsAny<ClientEntity>(), default), Times.Once);
    }

    [Fact]
    public async Task Execute_ShouldThrowException_WhenEmailAlreadyExists()
    {
        // Arrange
        var request = new CreateClientRequest(
            "Bob",
            "Johnson",
            "bob.johnson@example.com"
        );

        var existingClient = ClientEntity.Create(
            Guid.NewGuid(),
            PersonName.Create("Bob", "Johnson"),
            Email.Create("bob.johnson@example.com")
        );

        _clientRepositoryMock
            .Setup(x => x.GetByEmailAsync(request.Email, default))
            .ReturnsAsync(existingClient);

        // Act
        var act = async () => await _sut.Execute(request);

        // Assert
        await act.Should().ThrowAsync<Exception>()
            .WithMessage("Client with email bob.johnson@example.com already exists");

        _clientRepositoryMock.Verify(x => x.AddAsync(It.IsAny<ClientEntity>(), default), Times.Never);
    }

    [Fact]
    public async Task Execute_ShouldCreateClient_WithDifferentEmail()
    {
        // Arrange
        var request = new CreateClientRequest(
            "Carol",
            "Williams",
            "carol.williams@company.com"
        );

        _clientRepositoryMock
            .Setup(x => x.GetByEmailAsync(request.Email, default))
            .ReturnsAsync((ClientEntity?)null);

        _clientRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<ClientEntity>(), default))
            .ReturnsAsync((ClientEntity client, CancellationToken _) => client);

        // Act
        var result = await _sut.Execute(request);

        // Assert
        result.Should().NotBeNull();
        result.GetEmail().ToString().Should().Be("carol.williams@company.com");
    }

    [Fact]
    public async Task Execute_ShouldLogInformation_WhenClientIsCreated()
    {
        // Arrange
        var request = new CreateClientRequest(
            "David",
            "Brown",
            "david.brown@email.com"
        );

        _clientRepositoryMock
            .Setup(x => x.GetByEmailAsync(request.Email, default))
            .ReturnsAsync((ClientEntity?)null);

        _clientRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<ClientEntity>(), default))
            .ReturnsAsync((ClientEntity client, CancellationToken _) => client);

        // Act
        await _sut.Execute(request);

        // Assert
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Creating client with email")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }
}
