using System.Threading.Tasks;

namespace Miru.Hosting;

public interface IAppInitializer
{
    Task InitializeAsync();
}