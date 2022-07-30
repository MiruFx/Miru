using System.Threading;
using System.Threading.Tasks;

namespace Miru.Hosting;

public interface IMiruHost
{
    Task RunAsync(CancellationToken token = default);
}