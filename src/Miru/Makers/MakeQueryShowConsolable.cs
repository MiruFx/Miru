using System.CommandLine;
using System.Threading.Tasks;
using Miru.Consolables;
using Miru.Core;

namespace Miru.Makers;

public class MakeQueryShowConsolable : Consolable
{
    public MakeQueryShowConsolable() :
        base("make.query.show", "Make a new Query for showing")
    {
        Add(new Argument<string>("in"));
        Add(new Argument<string>("name"));
        Add(new Argument<string>("action"));
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

        public async Task Execute()
        {
            var make = new Maker(_solution);

            Console2.BreakLine();

            make.Query(In, Name, Action, "Show");

            await Task.CompletedTask;
        }
    }
}