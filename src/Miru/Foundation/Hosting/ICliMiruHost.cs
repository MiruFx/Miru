using System.Threading.Tasks;

namespace Miru.Foundation.Hosting
{
    public interface ICliMiruHost : IMiruHost
    {
        Task RunAsync(string args);
    }
}