using Devices.Application.Devices.Shared.Dtos;
using Devices.Application.Devices.Update.Commands;
using Devices.Application.Tests.Devices.Shared.Builders;

namespace Devices.Application.Tests.Devices.Update.Commands.Builders;

public class UpdateDeviceCommandBuilder
{
    private DeviceDto dto;

    public UpdateDeviceCommandBuilder()
    {
        dto = new DeviceDtoBuilder()
            .Build();
    }

    public UpdateDeviceCommand Build()
    {
        return new UpdateDeviceCommand(dto);
    }

    public UpdateDeviceCommandBuilder WithDeviceDto(DeviceDto dto)
    {
        this.dto = dto;
        return this;
    }
}
