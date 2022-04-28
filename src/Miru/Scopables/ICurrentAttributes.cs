using System.Threading;
using System.Threading.Tasks;

namespace Miru.Scopables;

public interface ICurrentAttributes
{
    Task BeforeAsync<TRequest>(TRequest request, CancellationToken ct);

    public Task AfterAsync<TRequest>(TRequest request, CancellationToken ct)
    {
        return Task.CompletedTask;
    }
}