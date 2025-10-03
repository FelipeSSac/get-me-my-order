namespace Order.Domain.ValueObject;

public sealed class PersonName : IEquatable<PersonName>
{
    private string FirstName { get; }
    private string LastName { get; }

    private PersonName(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }

    public static PersonName Create(string firstName, string lastName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("First name cannot be empty", nameof(firstName));

        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Last name cannot be empty", nameof(lastName));

        firstName = firstName.Trim();
        lastName = lastName.Trim();

        if (firstName.Length < 2)
            throw new ArgumentException("First name must be at least 2 characters", nameof(firstName));

        if (lastName.Length < 2)
            throw new ArgumentException("Last name must be at least 2 characters", nameof(lastName));

        if (firstName.Length > 50)
            throw new ArgumentException("First name cannot exceed 50 characters", nameof(firstName));

        if (lastName.Length > 50)
            throw new ArgumentException("Last name cannot exceed 50 characters", nameof(lastName));

        return new PersonName(firstName, lastName);
    }
    
    public static PersonName Create(string name)
    {
        var nameParts = name.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);

        if (nameParts.Length < 2)
            throw new ArgumentException("Full name must contain first and last name", nameof(name));

        return Create(nameParts[0], nameParts[1]);
    }

    public string GetFullName() => $"{FirstName} {LastName}";
    public string GetFirstName()  => FirstName;
    public string GetLastName()  => LastName;
    
    public bool Equals(PersonName? other)
    {
        if (other is null) return false;
        return FirstName == other.FirstName && LastName == other.LastName;
    }

    public override bool Equals(object? obj) => obj is PersonName other && Equals(other);

    public override int GetHashCode() => HashCode.Combine(FirstName, LastName);

    public override string ToString() => GetFullName();

    public static bool operator ==(PersonName? left, PersonName? right) =>
        left is null ? right is null : left.Equals(right);

    public static bool operator !=(PersonName? left, PersonName? right) => !(left == right);
}