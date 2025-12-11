namespace Devices.Shared.Extensions.FluentValidation;

public class KeyEqualityComparerValidator<T>(Func<T, object> keyExtractor) : IEqualityComparer<T>
{
    private readonly Func<T, object> _keyExtractor = keyExtractor;

    public bool Equals(T? x, T? y)
    {
        if (x is not null && y is not null)
            return _keyExtractor(x).Equals(_keyExtractor(y));

        return false;
    }

    public int GetHashCode(T obj)
    {
        var func = _keyExtractor(obj);
        if (func == null)
            return -1;

        return func.GetHashCode();
    }
}
