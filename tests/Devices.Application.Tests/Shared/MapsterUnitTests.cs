using Devices.Application.Devices.Shared.Mappers;

namespace Devices.Application.Tests.Shared;

public class MapsterUnitTests
{
    public static void ConfigurarMappers() =>
        DeviceMappings.AddDeviceMappings(null!);
}
