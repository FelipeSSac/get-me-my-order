using FluentAssertions;
using Order.Domain.ValueObject;

namespace Order.Tests.Domain.ValueObject;

public class PersonNameTests
{
    [Fact]
    public void Create_ShouldCreatePersonName_WhenValidParameters()
    {
        // Act
        var name = PersonName.Create("John", "Doe");

        // Assert
        name.Should().NotBeNull();
        name.FullName.Should().Be("John Doe");
    }

    [Fact]
    public void Create_ShouldThrowException_WhenFirstNameIsEmpty()
    {
        // Act
        var act = () => PersonName.Create("", "Doe");

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("First name cannot be empty*");
    }

    [Fact]
    public void Create_ShouldThrowException_WhenLastNameIsEmpty()
    {
        // Act
        var act = () => PersonName.Create("John", "");

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Last name cannot be empty*");
    }

    [Fact]
    public void Create_ShouldThrowException_WhenFirstNameIsTooShort()
    {
        // Act
        var act = () => PersonName.Create("J", "Doe");

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("First name must be at least 2 characters*");
    }

    [Fact]
    public void Create_ShouldThrowException_WhenLastNameIsTooShort()
    {
        // Act
        var act = () => PersonName.Create("John", "D");

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Last name must be at least 2 characters*");
    }

    [Fact]
    public void Create_ShouldThrowException_WhenFirstNameExceedsMaxLength()
    {
        // Arrange
        var longName = new string('a', 51);

        // Act
        var act = () => PersonName.Create(longName, "Doe");

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("First name cannot exceed 50 characters*");
    }

    [Fact]
    public void Create_ShouldThrowException_WhenLastNameExceedsMaxLength()
    {
        // Arrange
        var longName = new string('a', 51);

        // Act
        var act = () => PersonName.Create("John", longName);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Last name cannot exceed 50 characters*");
    }

    [Fact]
    public void Create_ShouldTrimWhitespace()
    {
        // Act
        var name = PersonName.Create("  John  ", "  Doe  ");

        // Assert
        name.FullName.Should().Be("John Doe");
    }

    [Fact]
    public void Create_FromFullName_ShouldSplitCorrectly()
    {
        // Act
        var name = PersonName.Create("John Doe");

        // Assert
        name.FullName.Should().Be("John Doe");
    }

    [Fact]
    public void Create_FromFullName_ShouldHandleMultipleSpaces()
    {
        // Act
        var name = PersonName.Create("John   Doe   Smith");

        // Assert
        name.FullName.Should().Be("John Doe   Smith");
    }

    [Fact]
    public void Create_FromFullName_ShouldThrowException_WhenOnlyOneName()
    {
        // Act
        var act = () => PersonName.Create("John");

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Full name must contain first and last name*");
    }

    [Fact]
    public void Equals_ShouldReturnTrue_WhenSameName()
    {
        // Arrange
        var name1 = PersonName.Create("John", "Doe");
        var name2 = PersonName.Create("John", "Doe");

        // Act & Assert
        name1.Equals(name2).Should().BeTrue();
        (name1 == name2).Should().BeTrue();
    }

    [Fact]
    public void Equals_ShouldReturnFalse_WhenDifferentFirstName()
    {
        // Arrange
        var name1 = PersonName.Create("John", "Doe");
        var name2 = PersonName.Create("Jane", "Doe");

        // Act & Assert
        name1.Equals(name2).Should().BeFalse();
        (name1 != name2).Should().BeTrue();
    }

    [Fact]
    public void Equals_ShouldReturnFalse_WhenDifferentLastName()
    {
        // Arrange
        var name1 = PersonName.Create("John", "Doe");
        var name2 = PersonName.Create("John", "Smith");

        // Act & Assert
        name1.Equals(name2).Should().BeFalse();
    }

    [Fact]
    public void ToString_ShouldReturnFullName()
    {
        // Arrange
        var name = PersonName.Create("John", "Doe");

        // Act
        var result = name.ToString();

        // Assert
        result.Should().Be("John Doe");
    }

    [Fact]
    public void GetHashCode_ShouldBeSame_ForEqualNames()
    {
        // Arrange
        var name1 = PersonName.Create("John", "Doe");
        var name2 = PersonName.Create("John", "Doe");

        // Act & Assert
        name1.GetHashCode().Should().Be(name2.GetHashCode());
    }
}