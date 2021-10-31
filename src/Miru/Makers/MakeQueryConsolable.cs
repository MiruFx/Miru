using System.CommandLine;
using System.Threading.Tasks;
using Miru.Consolables;
using Miru.Core;

namespace Miru.Makers;

public class MakeQueryConsolable : Consolable
{
    public MakeQueryConsolable() :
        base("make.query", "Make a new Query")
    {
        Add(new Argument<string>("in"));
        Add(new Argument<string>("name"));
        Add(new Argument<string>("action"));
        // TODO: add validator
        Add(new Option<string>("--template", "Which template to make? List or Show data"));
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
        public string Template { get; set; }

        public async Task Execute()
        {
            var make = new Maker(_solution);

            Console2.BreakLine();

            make.Query(In, Name, Action, Template);

            await Task.CompletedTask;
        }
    }
}