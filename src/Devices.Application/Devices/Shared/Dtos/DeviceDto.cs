using Devices.Domain.Devices.Enums;
using Devices.Domain.Devices.Specifications;
using FluentValidation;

namespace Devices.Application.Devices.Shared.Dtos;

public record DeviceDto(Guid Id, string Name, string Brand, DeviceState State, DateTime CreationTime);

public class DeviceDtoValidator : AbstractValidator<DeviceDto>
{
	public DeviceDtoValidator()
	{
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(DeviceSpecification.NameColumnSize);

        RuleFor(x => x.Brand)
            .NotEmpty()
            .MaximumLength(DeviceSpecification.BrandColumnSize);

        RuleFor(x => x.State)
            .IsInEnum();
    }
}