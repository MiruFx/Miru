using System.Threading.Tasks;

namespace Miru.Hosting;

public interface IHostInitialized
{
    Task InitializeAsync();
}