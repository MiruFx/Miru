using System.Threading.Tasks;
using Miru.Foundation.Hosting;

namespace Skeleton
{
    public class Program
    {
        public static async Task Main(string[] args) =>
            await MiruHost
                .CreateMiruHost<Startup>(args)
                .RunMiruAsync();
    }
}
