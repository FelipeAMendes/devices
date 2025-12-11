using FluentValidation.Results;

namespace Devices.Shared.Validations.Interfaces;

public interface IBaseValidation
{
    IList<ValidationFailure> Errors { get; }
}
