using Devices.Application.Devices.Shared.Dtos;
using Devices.Shared.Commands;
using FluentValidation;

namespace Devices.Application.Devices.Update.Commands;

public record UpdateDeviceCommandResult(Guid Id) : ICommandResult;

public record UpdateDeviceCommand(DeviceDto Device) : ICommand<CommandResult<UpdateDeviceCommandResult>>;

public class UpdateDeviceCommandValidator : AbstractValidator<UpdateDeviceCommand>
{
    public UpdateDeviceCommandValidator()
    {
        RuleFor(x => x.Device).SetValidator(new DeviceDtoValidator());
    }
}
