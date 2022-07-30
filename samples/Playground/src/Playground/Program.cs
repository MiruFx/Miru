using System.Threading.Tasks;
using Miru.Hosting;

namespace Playground
{
    public class Program
    {
        public static async Task Main(string[] args) =>
            await MiruHost
                .CreateMiruWebHost<Startup>(args)
                .RunMiruAsync();
    }
}
