namespace Devices.Application.Tests.Shared.Builders;

public class CancellationTokenBuilder
{
    public CancellationToken Build()
    {
        var ct = new CancellationToken();
        return ct;
    }
}
