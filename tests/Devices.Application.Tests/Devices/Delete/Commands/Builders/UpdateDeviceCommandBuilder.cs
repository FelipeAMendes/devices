using Devices.Application.Devices.Delete.Commands;

namespace Devices.Application.Tests.Devices.Delete.Commands.Builders;

public class DeleteDeviceCommandBuilder
{
    private Guid id;

    public DeleteDeviceCommandBuilder()
    {
        id = Guid.NewGuid();
    }

    public DeleteDeviceCommand Build()
    {
        return new DeleteDeviceCommand(id);
    }

    public DeleteDeviceCommandBuilder WithId(Guid id)
    {
        this.id = id;
        return this;
    }
}
