using Devices.Application.Devices.Create.Commands;
using Devices.Application.Devices.Shared.Dtos;
using FluentValidation.TestHelper;

namespace Devices.Application.Tests.Devices.Create.Commands;

[Trait("Commands", nameof(CreateDeviceCommand))]
public class CreateDeviceCommandTests
{
    [Fact]
    public void ShouldValidateCommand()
    {
        // Arrange
        var validator = new CreateDeviceCommandValidator();

        // Act/Assert
        validator.ShouldHaveChildValidator(
            x => x.Device,
            typeof(DeviceDtoValidator)
        );
    }
}
