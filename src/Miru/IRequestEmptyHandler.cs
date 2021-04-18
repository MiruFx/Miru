using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Miru
{
    public interface IRequestEmptyHandler<in TRequest, TResponse> : 
        IRequestHandler<TRequest, TResponse> where TRequest : IRequest<TResponse> where TResponse : new()
    {
        new Task<TResponse> Handle(TRequest request, CancellationToken ct) => Task.FromResult(new TResponse());
    }
}