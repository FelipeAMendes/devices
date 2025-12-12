using MediatR;

namespace Devices.Shared.Queries;

public interface IQuery<out TResponse> : IRequest<TResponse>
    where TResponse : notnull
{ }
