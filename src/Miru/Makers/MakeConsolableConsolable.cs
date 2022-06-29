using System.CommandLine;
using System.Threading.Tasks;
using Miru.Consolables;
using Miru.Core;

namespace Miru.Makers;

public class MakeConsolableConsolable : Consolable
{
    public MakeConsolableConsolable() :
        base("make.consolable", "Make a Consolable")
    {
        Add(new Argument<string>("name"));
    }

    public class ConsolableHandler : IConsolableHandler
    {
        private readonly MiruSolution _solution;
            
        public string Name { get; set; }
            
        public ConsolableHandler(MiruSolution solution)
        {
            _solution = solution;
        }
    
        public async Task Execute()
        {
            var make = new Maker(_solution);
            
            Console2.BreakLine();
    
            make.Consolable(Name);
            
            Console2.BreakLine();

            await Task.CompletedTask;
        }
    }
}