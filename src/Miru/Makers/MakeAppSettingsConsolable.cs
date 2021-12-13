using System.CommandLine;
using System.Threading.Tasks;
using Miru.Consolables;
using Miru.Core;

namespace Miru.Makers
{
    public class MakeAppSettingsConsolable : Consolable
    {
        public MakeAppSettingsConsolable() :
            base("make.app-settings", "Make a new appSettings.yml")
        {
            Add(new Argument<string>("environment"));
        }

        public class ConsolableHandler : IConsolableHandler
        {
            public string Environment { get; set; }
            
            private readonly MiruSolution _solution;

            public ConsolableHandler(MiruSolution solution)
            {
                _solution = solution;
            }

            public Task Execute()
            {
                var maker = new Maker(_solution);
            
                Console2.BreakLine();
                
                maker.AppSettings(Environment);

                Console2.BreakLine();
                
                return Task.CompletedTask;
            }
        }
    }
}