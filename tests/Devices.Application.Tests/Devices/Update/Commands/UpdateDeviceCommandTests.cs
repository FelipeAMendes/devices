using Devices.Application.Devices.Shared.Dtos;
using Devices.Application.Devices.Update.Commands;
using FluentValidation.TestHelper;

namespace Devices.Application.Tests.Devices.Update.Commands;

[Trait("Commands", nameof(UpdateDeviceCommand))]
public class UpdateDeviceCommandTests
{
    [Fact]
    public void ShouldValidateCommand()
    {
        // Arrange
        var validator = new UpdateDeviceCommandValidator();

        // Act/Assert
        validator.ShouldHaveChildValidator(
            x => x.Device,
            typeof(DeviceDtoValidator)
        );
    }
}
