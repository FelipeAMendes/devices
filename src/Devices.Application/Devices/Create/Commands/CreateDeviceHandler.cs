using Devices.Domain.Devices.Entities;
using Devices.Domain.Devices.Repositories;
using Devices.Shared.Commands;
using Mapster;

namespace Devices.Application.Devices.Create.Commands;

public class CreateDeviceHandler(IDeviceRepository repository) : ICommandHandler<CreateDeviceCommand, CommandResult<CreateDeviceCommandResult>>
{
    public async Task<CommandResult<CreateDeviceCommandResult>> Handle(CreateDeviceCommand command, CancellationToken ct)
    {
        var device = command.Device.Adapt<Device>();

        if (device.Validate() is var validationResult && !validationResult.IsValid)
            return CommandResult<CreateDeviceCommandResult>.BadRequest(validationResult.Errors);

        if (await repository.CreateAsync(device, ct) is var deviceCreated && !deviceCreated)
            return CommandResult<CreateDeviceCommandResult>.BadRequest("An error has ocurred when trying to create a new device.");

        return CommandResult<CreateDeviceCommandResult>.Created();
    }
}
