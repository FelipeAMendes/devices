namespace Devices.Shared.Extensions.StringExtensions;

public static class StringExtensions
{
    public static bool HasValue(this string value)
    {
        return !string.IsNullOrEmpty(value);
    }

    public static TEnum ToEnum<TEnum>(this string value) where TEnum : struct
    {
        if (Enum.TryParse<TEnum>(value, out var type))
            return type;

        return default;
    }
}
