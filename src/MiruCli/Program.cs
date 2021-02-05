using System.CommandLine;
using System.CommandLine.Invocation;
using System.Threading.Tasks;

namespace MiruCli
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var rootCommand = new RootCommand
            {
                new ShowProjectCommand("app", m => m.AppDir),
                new ShowProjectCommand("tests", m => m.AppTestsDir),
                new ShowProjectCommand("pagetests", m => m.AppPageTestsDir),
                new SetupCommand("setup"),
                new NewCommand("new"),
                new WatchCommand("watch", m => m.AppDir),
            };

            rootCommand.Name = "miru";
            
            await rootCommand.InvokeAsync(args);
        }
    }
}
