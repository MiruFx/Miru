using System.CommandLine;
using System.Threading.Tasks;
using Miru.Consolables;
using Miru.Core;

namespace Miru.Makers;

public class MakeQueryListConsolable : Consolable
{
    public MakeQueryListConsolable() :
        base("make.query.list", "Make a new Query for listing")
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

            make.Query(In, Name, Action, "List");

            await Task.CompletedTask;
        }
    }
}