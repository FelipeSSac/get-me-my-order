using FluentAssertions;
using Order.Domain.ValueObject;

namespace Order.Tests.Domain.ValueObject;

public class EmailTests
{
    [Theory]
    [InlineData("test@example.com")]
    [InlineData("user.name@example.com")]
    [InlineData("user+tag@example.co.uk")]
    [InlineData("test123@subdomain.example.com")]
    public void Create_ShouldCreateEmail_WhenValidFormat(string emailAddress)
    {
        // Act
        var email = Email.Create(emailAddress);

        // Assert
        email.Should().NotBeNull();
        email.ToString().Should().Be(emailAddress.ToLowerInvariant());
    }

    [Fact]
    public void Create_ShouldThrowException_WhenEmailIsEmpty()
    {
        // Act
        var act = () => Email.Create("");

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Email cannot be empty*");
    }

    [Fact]
    public void Create_ShouldThrowException_WhenEmailIsNull()
    {
        // Act
        var act = () => Email.Create(null!);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Email cannot be empty*");
    }

    [Theory]
    [InlineData("invalid")]
    [InlineData("invalid@")]
    [InlineData("@example.com")]
    [InlineData("invalid@.com")]
    [InlineData("invalid email@example.com")]
    public void Create_ShouldThrowException_WhenInvalidFormat(string invalidEmail)
    {
        // Act
        var act = () => Email.Create(invalidEmail);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage($"Invalid email format: {invalidEmail.Trim().ToLowerInvariant()}*");
    }

    [Fact]
    public void Create_ShouldThrowException_WhenEmailExceedsMaxLength()
    {
        // Arrange
        var longEmail = new string('a', 250) + "@example.com"; // Total > 254

        // Act
        var act = () => Email.Create(longEmail);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Email exceeds maximum length of 254 characters*");
    }

    [Fact]
    public void Create_ShouldConvertToLowerCase()
    {
        // Act
        var email = Email.Create("TEST@EXAMPLE.COM");

        // Assert
        email.ToString().Should().Be("test@example.com");
    }

    [Fact]
    public void Create_ShouldTrimWhitespace()
    {
        // Act
        var email = Email.Create("  test@example.com  ");

        // Assert
        email.ToString().Should().Be("test@example.com");
    }

    [Fact]
    public void Equals_ShouldReturnTrue_WhenSameEmail()
    {
        // Arrange
        var email1 = Email.Create("test@example.com");
        var email2 = Email.Create("test@example.com");

        // Act & Assert
        email1.Equals(email2).Should().BeTrue();
        (email1 == email2).Should().BeTrue();
    }

    [Fact]
    public void Equals_ShouldReturnFalse_WhenDifferentEmail()
    {
        // Arrange
        var email1 = Email.Create("test1@example.com");
        var email2 = Email.Create("test2@example.com");

        // Act & Assert
        email1.Equals(email2).Should().BeFalse();
        (email1 != email2).Should().BeTrue();
    }

    [Fact]
    public void Equals_ShouldBeCaseInsensitive()
    {
        // Arrange
        var email1 = Email.Create("TEST@EXAMPLE.COM");
        var email2 = Email.Create("test@example.com");

        // Act & Assert
        email1.Equals(email2).Should().BeTrue();
    }

    [Fact]
    public void GetHashCode_ShouldBeSame_ForEqualEmails()
    {
        // Arrange
        var email1 = Email.Create("test@example.com");
        var email2 = Email.Create("test@example.com");

        // Act & Assert
        email1.GetHashCode().Should().Be(email2.GetHashCode());
    }
}