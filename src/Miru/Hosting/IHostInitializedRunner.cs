using System.Threading.Tasks;

namespace Miru.Hosting;

public interface IHostInitializedRunner
{
    Task RunAsync();
}