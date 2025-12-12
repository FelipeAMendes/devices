using Devices.Application.Devices.Shared.Dtos;
using Devices.Application.Tests.Devices.Shared.Builders;
using Devices.Domain.Devices.Enums;
using Devices.Domain.Devices.Specifications;
using FluentValidation.TestHelper;

namespace Devices.Application.Tests.Devices.Shared.Validators;

[Trait("Validators", nameof(DeviceDtoValidator))]
public class DeviceDtoValidatorTests
{
    private readonly DeviceDtoValidator _validator;

    public DeviceDtoValidatorTests()
    {
        _validator = new DeviceDtoValidator();
    }

    [Fact]
    public void ShouldValidateCommand()
    {
        // Arrange
        var dto = new DeviceDtoBuilder()
            .Build();

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void ShouldReturnError_WhenNameIsNotProvided()
    {
        // Arrange
        var dto = new DeviceDtoBuilder()
            .WithName(default!)
            .Build();

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void ShouldReturnError_WhenNameIsInvalid()
    {
        // Arrange
        var dto = new DeviceDtoBuilder()
            .WithName(new string('a', DeviceSpecification.NameColumnSize + 1))
            .Build();

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void ShouldReturnError_WhenBrandIsInvalid()
    {
        // Arrange
        var dto = new DeviceDtoBuilder()
            .WithBrand(new string('a', DeviceSpecification.NameColumnSize + 1))
            .Build();

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Brand);
    }

    [Fact]
    public void ShouldReturnError_WhenBrandIsNotProvided()
    {
        // Arrange
        var dto = new DeviceDtoBuilder()
            .WithBrand(default!)
            .Build();

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Brand);
    }

    [Fact]
    public void ShouldReturnError_WhenStateIsInvalid()
    {
        // Arrange
        var dto = new DeviceDtoBuilder()
            .WithState((DeviceState)99)
            .Build();

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.State);
    }
}
