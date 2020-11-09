using System.CommandLine;
using System.CommandLine.Invocation;
using System.Threading.Tasks;
using Miru.Tests.CommandLine;

namespace Miru.Tests
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var rootCommand = new RootCommand
            {
                new TestNewSolutionCommand()
            };

            await rootCommand.InvokeAsync(args);
        }
    }
}