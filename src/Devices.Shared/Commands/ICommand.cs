using MediatR;

namespace Devices.Shared.Commands;

public interface ICommand : ICommand<Unit> { }

public interface ICommand<out TResponse> : IRequest<TResponse> { }
