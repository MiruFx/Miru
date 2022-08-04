using System.Threading.Tasks;

namespace Miru.Hosting;

public interface IAppInitializerRunner
{
    Task RunAsync();
}