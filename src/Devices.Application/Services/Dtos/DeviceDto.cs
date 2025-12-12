using Devices.Domain.Devices.Enums;
using Devices.Domain.Devices.Specifications;
using FluentValidation;

namespace Devices.Application.Services.Dtos;

public record DeviceDto(string Name, string Brand, DeviceState State);

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