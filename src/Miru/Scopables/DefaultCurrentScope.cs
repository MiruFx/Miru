using System.Threading;
using System.Threading.Tasks;

namespace Miru.Scopables;

public class DefaultCurrentScope : ICurrentScope
{
    public async Task BeforeAsync<TRequest>(TRequest request, CancellationToken ct)
    {
        await Task.CompletedTask;
    }
}