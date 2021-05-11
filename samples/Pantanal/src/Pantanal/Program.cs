using System.Threading.Tasks;
using Miru.Foundation.Hosting;

namespace Pantanal
{
    public class Program
    {
        public static async Task Main(string[] args) =>
            await MiruHost
                .CreateMiruWebHost<Startup>(args)
                .RunMiruAsync();
    }
}
