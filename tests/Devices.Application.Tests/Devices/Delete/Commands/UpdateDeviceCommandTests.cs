using Devices.Application.Devices.Delete.Commands;
using FluentValidation.TestHelper;

namespace Devices.Application.Tests.Devices.Delete.Commands;

[Trait("Commands", nameof(DeleteDeviceCommand))]
public class DeleteDeviceCommandTests
{
    private readonly DeleteDeviceCommandValidator _validator;

    public DeleteDeviceCommandTests()
    {
        _validator = new DeleteDeviceCommandValidator();
    }

    [Fact]
    public void ShouldValidateCommand()
    {
        // Arrange
        var command = new DeleteDeviceCommand(Guid.NewGuid());

        // Act
        var validationResult = _validator.TestValidate(command);

        // Assert
        validationResult.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void ShouldReturnErrorCommand_WhenIdNotProvided()
    {
        // Arrange
        var command = new DeleteDeviceCommand(Guid.Empty);

        // Act
        var validationResult = _validator.TestValidate(command);

        // Assert
        validationResult.ShouldHaveValidationErrorFor(x => x.Id);
    }
}
