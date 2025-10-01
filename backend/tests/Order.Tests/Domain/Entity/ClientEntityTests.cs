using FluentAssertions;
using Order.Domain.Entity;
using Order.Domain.ValueObject;

namespace Order.Tests.Domain.Entity;

public class ClientEntityTests
{
    [Fact]
    public void Create_ShouldCreateClient_WhenValidParameters()
    {
        // Arrange
        var id = Guid.NewGuid();
        var name = PersonName.Create("John", "Doe");
        var email = Email.Create("john@example.com");

        // Act
        var client = ClientEntity.Create(id, name, email);

        // Assert
        client.Should().NotBeNull();
        client.GetId().Should().Be(id);
    }

    [Fact]
    public void Create_ShouldCreateClient_WithNullId()
    {
        // Arrange
        var name = PersonName.Create("John", "Doe");
        var email = Email.Create("john@example.com");

        // Act
        var client = ClientEntity.Create(null, name, email);

        // Assert
        client.Should().NotBeNull();
        client.GetId().Should().BeNull();
    }

}