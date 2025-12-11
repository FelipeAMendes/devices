namespace Devices.Shared.Extensions.ObjectExtensions;

public static class ObjectExtensions
{
    public static bool IsNull(this object value)
    {
        return value is null;
    }
}
