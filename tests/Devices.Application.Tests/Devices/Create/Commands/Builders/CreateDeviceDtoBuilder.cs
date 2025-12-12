using Devices.Application.Devices.Create.Commands;
using Devices.Application.Devices.Shared.Dtos;
using Devices.Application.Tests.Devices.Shared.Builders;

namespace Devices.Application.Tests.Devices.Create.Commands.Builders;

public class CreateDeviceCommandBuilder
{
    private DeviceDto dto;

    public CreateDeviceCommandBuilder()
    {
        dto = new DeviceDtoBuilder()
            .Build();
    }

    public CreateDeviceCommand Build()
    {
        return new CreateDeviceCommand(dto);
    }

    public CreateDeviceCommandBuilder WithDeviceDto(DeviceDto dto)
    {
        this.dto = dto;
        return this;
    }
}
