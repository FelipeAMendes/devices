using Devices.Domain.Devices.Repositories;
using Devices.Shared.Commands;

namespace Devices.Application.Devices.Update.Commands;

public class UpdateDeviceHandler(IDeviceRepository repository) : ICommandHandler<UpdateDeviceCommand, CommandResult<UpdateDeviceCommandResult>>
{
    public async Task<CommandResult<UpdateDeviceCommandResult>> Handle(UpdateDeviceCommand command, CancellationToken ct)
    {
        if (await repository.GetByIdAsync(command.Device.Id, ct) is var existingDevice && existingDevice is null)
            return CommandResult<UpdateDeviceCommandResult>.NotFound();

        if (!existingDevice.CanUpdateWhenStateIsInUse(command.Device.Name, command.Device.Brand))
            return CommandResult<UpdateDeviceCommandResult>.BadRequest("Name and brand cannot be updated if state is in use.");

        existingDevice.Update(command.Device.Name, command.Device.Brand, command.Device.State);

        if (existingDevice.Validate() is var validationResult && !validationResult.IsValid)
            return CommandResult<UpdateDeviceCommandResult>.BadRequest(validationResult.Errors);

        if (await repository.UpdateAsync(existingDevice, ct) is var deviceUpdated && !deviceUpdated)
            return CommandResult<UpdateDeviceCommandResult>.BadRequest("An error has ocurred when trying to update the device.");

        return CommandResult<UpdateDeviceCommandResult>.NoContent();
    }
}
