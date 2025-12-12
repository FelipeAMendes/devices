using Devices.Shared.Commands;
using FluentValidation;

namespace Devices.Application.Devices.Delete.Commands;

public record DeleteDeviceCommandResult() : ICommandResult;

public record DeleteDeviceCommand(Guid Id) : ICommand<CommandResult<DeleteDeviceCommandResult>>;

public class DeleteDeviceCommandValidator : AbstractValidator<DeleteDeviceCommand>
{
    public DeleteDeviceCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
