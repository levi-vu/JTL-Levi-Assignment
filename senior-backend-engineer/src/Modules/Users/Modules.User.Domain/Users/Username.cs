namespace Modules.User.Domain.Users;

public sealed class Username : IEquatable<Username>
{
    public const int MaxLength = 100;

    private Username()
    {
        Value = null!;
        NormalizedValue = null!;
    }

    private Username(string value)
    {
        Value = value;
        NormalizedValue = value.ToUpperInvariant();
    }

    public string Value { get; private set; }

    public string NormalizedValue { get; private set; }

    public static Username Create(string? value)
    {
        var trimmedValue = value?.Trim();

        if (string.IsNullOrWhiteSpace(trimmedValue))
        {
            throw new ArgumentException("Username must not be empty.", nameof(value));
        }

        if (trimmedValue.Length > MaxLength)
        {
            throw new ArgumentException(
                $"Username must not exceed {MaxLength} characters.",
                nameof(value));
        }

        return new Username(trimmedValue);
    }

    public bool Equals(Username? other) =>
        other is not null &&
        StringComparer.Ordinal.Equals(NormalizedValue, other.NormalizedValue);

    public override bool Equals(object? obj) => obj is Username other && Equals(other);

    public override int GetHashCode() => StringComparer.Ordinal.GetHashCode(NormalizedValue);

    public override string ToString() => Value;
}