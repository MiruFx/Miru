using Miru.Hosting;

namespace Corpo.Skeleton;

public class Program
{
    public static async Task Main(string[] args) =>
        await MiruHost
            .CreateMiruWebHost<Startup>(args)
            .RunMiruAsync();
}