namespace Devices.Shared.Responses.Extensions;

public static class ResponseStatusExtensions
{
    public static bool Succeeded(this ResponseStatus status)
    {
        return status
            is ResponseStatus.Ok
            or ResponseStatus.Created
            or ResponseStatus.Accepted
            or ResponseStatus.NoContent;
    }
}