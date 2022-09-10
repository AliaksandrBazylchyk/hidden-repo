using MediatR;

namespace Commands
{
    public interface ICommand<out TResponse> : IRequest<TResponse>
    { }

    public interface ICommand : IRequest
    { }
}
