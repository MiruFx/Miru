using System.Threading.Tasks;
using Miru.Foundation.Hosting;

namespace Corpo.Skeleton
{
    public class Program
    {
        public static async Task Main(string[] args) =>
            await MiruHost
                .CreateMiruHost<Startup>(args)
                .RunMiruAsync();
    }
}
