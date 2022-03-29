using System.CommandLine;
using System.Threading.Tasks;
using Miru.Consolables;
using Miru.Core;

namespace Miru.Makers;

public class MakeJobConsolable : Consolable
{
    public MakeJobConsolable() :
        base("make.job", "Make a new Job")
    {
        Add(new Argument<string>("in"));
        Add(new Argument<string>("name"));
        Add(new Argument<string>("action"));
        Add(new Option<bool>("--no-tests"));
        Add(new Option<bool>("--only-tests"));
    }

    public class ConsolableHandler : IConsolableHandler
    {
        private readonly MiruSolution _solution;

        public ConsolableHandler(MiruSolution solution)
        {
            _solution = solution;
        }

        public string In { get; set; }
        public string Name { get; set; }
        public string Action { get; set; }
        public bool NoTests { get; set; }
        public bool OnlyTests { get; set; }

        public async Task Execute()
        {
            var make = new Maker(_solution);

            Console2.BreakLine();

            make.Job(In, Name, Action, OnlyTests, NoTests);
            
            Console2.BreakLine();

            await Task.CompletedTask;
        }
    }
}