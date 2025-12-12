using Devices.Shared.Commands;
using Devices.Shared.Queries;
using Devices.Shared.Responses;
using Devices.Shared.Responses.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Devices.Api.Controllers;

[ApiController]
public abstract class BaseController : Controller
{
    protected IActionResult GenerateResponse<T>(
        CommandResult<T> response,
        Func<CommandResult<T>, object> mapSuccessRequestBody,
        Func<CommandResult<T>, object> mapFailureRequestBody) where T : ICommandResult
    {
        object? obj = (response.Status.Succeeded()
            ? mapSuccessRequestBody
            : mapFailureRequestBody)?.Invoke(response);

        bool hasRequestBody = obj is not null;

        if (hasRequestBody)
        {
            IActionResult actionResult = new ObjectResult(obj)
            {
                StatusCode = (int)response.Status
            };
            return actionResult;
        }

        return new StatusCodeResult((int)response.Status);
    }

    protected IActionResult GenerateResponse<T>(
        QueryResult<T> response,
        Func<QueryResult<T>, object?> mapSuccessRequestBody,
        Func<QueryResult<T>, object?> mapFailureRequestBody) where T : IQueryResult
    {
        object? obj = (response.Status.Succeeded()
            ? mapSuccessRequestBody
            : mapFailureRequestBody)?.Invoke(response);

        bool hasRequestBody = obj is not null;

        if (hasRequestBody)
        {
            IActionResult actionResult = new ObjectResult(obj)
            {
                StatusCode = (int)response.Status
            };
            return actionResult;
        }

        return new StatusCodeResult((int)response.Status);
    }

    protected IActionResult GenerateResponse<T>(CommandResult<T> response, Func<CommandResult<T>, object> mapSuccessRequestBody) where T : ICommandResult
    {
        return GenerateResponse(response, mapSuccessRequestBody, (x) => x.Errors);
    }

    protected IActionResult GenerateResponse<T>(QueryResult<T> response, Func<QueryResult<T>, object?> mapSuccessRequestBody) where T : IQueryResult
    {
        return GenerateResponse(response, mapSuccessRequestBody, (x) => x.Message ?? string.Empty);
    }
}

public static class CommandResultExtensions
{
    public static IActionResult ToActionResult<T>(this ControllerBase controller, CommandResult<T> result)
        where T : ICommandResult
    {
        return result.Status switch
        {
            ResponseStatus.Ok => controller.Ok(result.Result),
            ResponseStatus.Created => controller.Created(),
            ResponseStatus.NoContent => controller.NoContent(),
            ResponseStatus.Unauthorized => controller.NotFound(new ApiErrorResponse
            {
                Message = "Unauthorized"
            }),
            ResponseStatus.Forbidden => controller.NotFound(new ApiErrorResponse
            {
                Message = "Forbidden"
            }),
            ResponseStatus.BadRequest => controller.BadRequest(new ApiErrorResponse
            {
                Message = "Validation error.",
                Errors = result.Errors.Select(e => new ApiErrorDetail
                {
                    Field = e?.PropertyName,
                    Message = e?.ErrorMessage
                }).ToList()
            }),
            ResponseStatus.NotFound => controller.NotFound(new ApiErrorResponse
            {
                Message = "Resource not found"
            }),
            _ => controller.StatusCode(500, new ApiErrorResponse
            {
                Message = "Unexpected server error"
            })
        };
    }
}
