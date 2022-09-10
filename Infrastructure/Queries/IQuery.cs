using MediatR;

namespace Queries
{
    public interface IQuery<out TResponse> : IRequest<TResponse>
    { }
}
