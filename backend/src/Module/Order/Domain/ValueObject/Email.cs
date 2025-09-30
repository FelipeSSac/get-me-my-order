using System.Text.RegularExpressions;

namespace Order.Domain.ValueObject;

public sealed class Email : IEquatable<Email>
{
    private const string EmailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

    private string Value { get; }

    private Email(string value)
    {
        Value = value;
    }

    public static Email Create(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be empty", nameof(email));

        email = email.Trim().ToLowerInvariant();

        if (!Regex.IsMatch(email, EmailPattern))
            throw new ArgumentException($"Invalid email format: {email}", nameof(email));

        if (email.Length > 254)
            throw new ArgumentException("Email exceeds maximum length of 254 characters", nameof(email));

        return new Email(email);
    }

    public bool Equals(Email? other)
    {
        if (other is null) return false;
        return Value == other.Value;
    }

    public override bool Equals(object? obj) => obj is Email other && Equals(other);

    public override int GetHashCode() => Value.GetHashCode();

    public override string ToString() => Value;

    public static bool operator ==(Email? left, Email? right) =>
        left is null ? right is null : left.Equals(right);

    public static bool operator !=(Email? left, Email? right) => !(left == right);
}