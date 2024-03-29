using System.CommandLine;
using System.Threading.Tasks;
using Miru.Consolables;
using Miru.Core;

namespace Miru.Makers;

public class MakeMailableConsolable : Consolable
{
    public MakeMailableConsolable() :
        base("make.mailable", "Make a new Mailable with email template")
    {
        Add(new Argument<string>("in"));
        Add(new Argument<string>("name"));
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

        public async Task Execute()
        {
            var make = new Maker(_solution);

            Console2.BreakLine();

            make.Mail(In, Name);
            
            Console2.BreakLine();

            await Task.CompletedTask;
        }
    }
}