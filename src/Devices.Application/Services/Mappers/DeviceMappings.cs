using Devices.Application.Services.Dtos;
using Devices.Domain.Devices.Entities;
using Mapster;
using Microsoft.Extensions.DependencyInjection;

namespace Devices.Application.Services.Mappers;

public static class DeviceMappings
{
    public static void AddDeviceMappings(this IServiceCollection _)
    {
        TypeAdapterConfig<DeviceDto, Device>.NewConfig().MapToConstructor(true);

        TypeAdapterConfig<Device, DeviceDto>.NewConfig().MapToConstructor(true);
    }
}
