using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Polly;
using Polly.Retry;

namespace Devices.Infrastructure.Data.Extensions;

public static class AppDbContextExtensions
{
    private const int CountRetry = 3;

    private static readonly AsyncRetryPolicy AsyncRetryPolicy = Policy
        .Handle<SqlException>(ex => SqlExceptionCodes.ErrorCodesToRetry.Contains(ex.Number))
        .WaitAndRetryAsync(CountRetry, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

    public static async Task ExecuteStrategyAndRetry(this DataContext _, DbContext dbContext, Func<Task> operation)
    {
        async Task ExecuteStrategy()
        {
            var strategy = dbContext.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(operation);
        }

        await AsyncRetryPolicy.ExecuteAsync(ExecuteStrategy);
    }

    public static async Task<TResult> ExecuteStrategyAndRetry<TResult>(this DataContext _, DbContext dbContext, Func<Task<TResult>> operation)
    {
        async Task<TResult> ExecuteStrategy()
        {
            var strategy = dbContext.Database.CreateExecutionStrategy();

            var result = await strategy.ExecuteAsync(operation);

            return result;
        }

        var executeRetry = await AsyncRetryPolicy.ExecuteAsync(ExecuteStrategy);

        return executeRetry;
    }

    public static async Task<TResult> ExecuteRetry<TResult>(this DataContext _, Func<Task<TResult>> operation)
    {
        var executionRetry = await AsyncRetryPolicy.ExecuteAsync(operation);

        return executionRetry;
    }
}

public static class SqlExceptionCodes
{
    private const int Timeout = -2;
    private const int Deadlock = 1205;
    private const int CouldNotOpenConnection = 53;
    private const int TransportFail = 121;

    public static List<int> ErrorCodesToRetry => new()
    {
        Timeout,
        Deadlock,
        CouldNotOpenConnection,
        TransportFail
    };
}
