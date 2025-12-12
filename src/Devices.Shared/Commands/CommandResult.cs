using Devices.Shared.Responses;
using FluentValidation.Results;
using System.Text.Json.Serialization;

namespace Devices.Shared.Commands;

public interface ICommandResult { }

public class CommandResult<TCommandResult> where TCommandResult : ICommandResult
{
    private readonly IList<ValidationFailure> _errors;

    public CommandResult(ResponseStatus status)
    {
        Status = status;
        Result = default;
        _errors ??= [];
    }

    public CommandResult(ResponseStatus status, IList<ValidationFailure> errors)
    {
        Status = status;
        _errors ??= [];
        _errors = errors;
    }

    public CommandResult(bool succeeded, IList<ValidationFailure> errors)
        : this(succeeded ? ResponseStatus.Ok : ResponseStatus.BadRequest, errors) { }

    public CommandResult(ResponseStatus success, TCommandResult result)
    {
        Status = success;
        Result = result;
        _errors ??= [];
    }

    public static CommandResult<TCommandResult> Fail(IEnumerable<ValidationFailure> errors) =>
        new(false, [.. errors]);

    [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
    public ResponseStatus Status { get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public TCommandResult? Result { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IEnumerable<ValidationFailure?> Errors =>
        _errors?.Select(x => new ValidationFailure(x.PropertyName, x.ErrorMessage)) ?? [];

    public static CommandResult<TCommandResult> Created()
    {
        return new CommandResult<TCommandResult>(ResponseStatus.Created);
    }

    public static CommandResult<TCommandResult> Accepted()
    {
        return new CommandResult<TCommandResult>(ResponseStatus.Accepted);
    }

    public static CommandResult<TCommandResult> BadRequest(IList<ValidationFailure> errors)
    {
        return new CommandResult<TCommandResult>(ResponseStatus.BadRequest, errors);
    }

    public static CommandResult<TCommandResult> BadRequest(string message)
    {
        var errorMessages = new List<ValidationFailure> { new("", message) };
        return new CommandResult<TCommandResult>(ResponseStatus.BadRequest, errorMessages);
    }

    public static CommandResult<TCommandResult> NoContent()
    {
        return new CommandResult<TCommandResult>(ResponseStatus.NoContent);
    }

    public static CommandResult<TCommandResult> NotFound()
    {
        return new CommandResult<TCommandResult>(ResponseStatus.NotFound);
    }

    public static CommandResult<TCommandResult> Ok(TCommandResult commandResult)
    {
        return new CommandResult<TCommandResult>(ResponseStatus.Ok, commandResult);
    }
}
