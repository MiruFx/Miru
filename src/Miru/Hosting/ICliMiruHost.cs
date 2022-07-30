using System.Threading.Tasks;

namespace Miru.Hosting
{
    public interface ICliMiruHost : IMiruHost
    {
        Task RunAsync(string args);
    }
}