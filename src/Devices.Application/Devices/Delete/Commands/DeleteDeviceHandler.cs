using Devices.Domain.Devices.Repositories;
using Devices.Shared.Commands;

namespace Devices.Application.Devices.Delete.Commands;

public class DeleteDeviceHandler(IDeviceRepository repository) : ICommandHandler<DeleteDeviceCommand, CommandResult<DeleteDeviceCommandResult>>
{
    public async Task<CommandResult<DeleteDeviceCommandResult>> Handle(DeleteDeviceCommand command, CancellationToken ct)
    {
        if (await repository.GetByIdAsync(command.Id, ct) is var existingDevice && existingDevice is null)
            return CommandResult<DeleteDeviceCommandResult>.NotFound();

        if (!existingDevice.CanDelete())
            return CommandResult<DeleteDeviceCommandResult>.BadRequest("In use devices cannot be deleted.");

        existingDevice.Delete();

        if (await repository.UpdateAsync(existingDevice, ct) is var deviceDeleted && !deviceDeleted)
            return CommandResult<DeleteDeviceCommandResult>.BadRequest("An error has ocurred when trying to delete the device.");

        return CommandResult<DeleteDeviceCommandResult>.NoContent();
    }
}
