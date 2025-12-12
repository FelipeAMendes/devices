using Devices.Application.Devices.Shared.Dtos;
using Devices.Domain.Devices.Entities;
using Mapster;
using Microsoft.Extensions.DependencyInjection;

namespace Devices.Application.Devices.Shared.Mappers;

public static class DeviceMappings
{
    public static void AddDeviceMappings(this IServiceCollection _)
    {
        TypeAdapterConfig<DeviceDto, Device>.NewConfig().MapWith(dto => new Device(dto.Name, dto.Brand, dto.State));

        TypeAdapterConfig<Device, DeviceDto>.NewConfig().MapToConstructor(true);
    }
}
