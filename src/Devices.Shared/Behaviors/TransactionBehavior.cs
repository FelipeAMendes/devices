using Devices.Application.Data;
using Devices.Shared.Commands;
using Devices.Shared.Responses.Extensions;
using MediatR;

namespace Devices.Shared.Behaviors;

public class TransactionBehavior<TRequest, TResponse>(IDataContext dataContext) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICommand<TResponse>
    where TResponse : ICommandResult
{
    private readonly IDataContext _dataContext = dataContext;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        await BeginTransactionAsync();

        var response = await next(cancellationToken);

        await CommitTransactionAsync(response);

        return response;
    }

    private async Task BeginTransactionAsync()
    {
        await _dataContext.BeginTransactionAsync();
    }

    private async Task CommitTransactionAsync(TResponse response)
    {
        try
        {
            if (response is CommandResult<ICommandResult> commandResponse && commandResponse.Status.Succeeded())
            {
                await _dataContext.CommitAsync();
                return;
            }

            await _dataContext.RollbackAsync();
        }
        catch (Exception)
        {
            await _dataContext.RollbackAsync();
        }
    }
}