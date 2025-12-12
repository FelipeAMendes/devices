using Devices.Application.Devices.Shared.Dtos;
using Devices.Shared.Commands;
using FluentValidation;

namespace Devices.Application.Devices.Create.Commands;

public record CreateDeviceCommandResult() : ICommandResult;

public record CreateDeviceCommand(DeviceDto Device) : ICommand<CommandResult<CreateDeviceCommandResult>>;

public class CreateDeviceCommandValidator : AbstractValidator<CreateDeviceCommand>
{
    public CreateDeviceCommandValidator()
    {
        RuleFor(x => x.Device).SetValidator(new DeviceDtoValidator());
    }
}
