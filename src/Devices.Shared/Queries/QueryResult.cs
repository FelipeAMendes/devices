using Devices.Shared.Responses;
using System.Text.Json.Serialization;

namespace Devices.Shared.Queries;

public interface IQueryResult { }

public class QueryResult<TQueryResult> where TQueryResult : IQueryResult
{
    public QueryResult(ResponseStatus status)
    {
        Status = status;
    }

    public QueryResult(ResponseStatus status, string message)
    {
        Status = status;
        Message = message;
    }

    public QueryResult(ResponseStatus success, TQueryResult result)
    {
        Status = success;
        Result = result;
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
    public ResponseStatus Status { get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Message { get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public TQueryResult? Result { get; set; }

    public static QueryResult<TQueryResult> NoContent()
    {
        return new QueryResult<TQueryResult>(ResponseStatus.NoContent);
    }

    public static QueryResult<TQueryResult> NotFound()
    {
        return new QueryResult<TQueryResult>(ResponseStatus.NotFound);
    }

    public static QueryResult<TQueryResult> Ok(TQueryResult queryResult)
    {
        return new QueryResult<TQueryResult>(ResponseStatus.Ok, queryResult);
    }
}
