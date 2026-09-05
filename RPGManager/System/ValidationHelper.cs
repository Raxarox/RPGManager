namespace RPGManager.System;

public static class ValidationHelper
{
    public static void ValidatePositiveValue(string name, string itemType, string propertyName, int value)
    {
        if (value < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(propertyName),
                $"{itemType} '{name}' cannot have a negative {propertyName}.");
        }
    }

    public static void ValidatePositiveValue(string name, string itemType, string propertyName, decimal value)
    {
        if (value < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(propertyName),
                $"{itemType} '{name}' cannot have a negative {propertyName}.");
        }
    }

    public static void ValidateRequiredString(string name, string itemType, string propertyName, string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException($"{itemType} '{name}' must have a valid {propertyName}.", nameof(propertyName));
        }
    }

    public static void ValidatePositiveInteger(string name, string itemType, string propertyName, int value)
    {
        if (value <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(propertyName),
                $"{itemType} '{name}' must have a positive {propertyName}.");
        }
    }
}
