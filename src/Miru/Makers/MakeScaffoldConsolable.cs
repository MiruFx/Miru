using System.CommandLine;
using System.Threading.Tasks;
using Miru.Consolables;
using Miru.Core;

namespace Miru.Makers
{
    public class MakeScaffoldConsolable : Consolable
    {
        public MakeScaffoldConsolable() :
            base("make.scaffold", "Make scaffold with new new, edit, list, and delete features")
        {
            Add(new Argument<string>("in"));
            Add(new Argument<string>("name"));
        }

        public class ConsolableHandler : IConsolableHandler
        {
            public string In { get; set; }
            public string Name { get; set; }
            
            private readonly MiruSolution _solution;

            public ConsolableHandler(MiruSolution solution)
            {
                _solution = solution;
            }

            public Task Execute()
            {
                var maker = new Maker(_solution);
            
                Console2.BreakLine();
                
                maker.Scaffold(In, Name);
                
                Console2.BreakLine();
                Console2.WhiteLine($"Consider creating an Entity and a Migration for {Name}");
                Console2.BreakLine();
                
                return Task.CompletedTask;
            }
        }
    }
}