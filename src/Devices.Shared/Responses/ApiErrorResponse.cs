namespace Devices.Shared.Responses;

public record ApiErrorResponse
{
    public string? Message { get; init; }
    public List<ApiErrorDetail> Errors { get; init; } = new();
}

public record ApiErrorDetail
{
    public string? Field { get; init; }
    public string? Message { get; init; }
}
