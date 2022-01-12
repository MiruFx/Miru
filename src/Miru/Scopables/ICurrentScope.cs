using System.Threading;
using System.Threading.Tasks;

namespace Miru.Scopables;

public interface ICurrentScope
{
    Task BeforeAsync(CancellationToken ct);

    public Task AfterAsync(CancellationToken ct)
    {
        return Task.CompletedTask;
    }
}