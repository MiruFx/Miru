using Miru.Consolables;
using Miru.Core;
using Oakton;

namespace Miru.Makers
{
    [Description("Make new, edit, show, and list Features ", Name = "make:crud")]
    public class MakeCrudConsolable : ConsolableSync<MakeCrudConsolable.Input>
    {
        private readonly MiruSolution _solution;

        public MakeCrudConsolable(MiruSolution solution)
        {
            _solution = solution;
        }

        public class Input
        {
            public string In { get; set; }

            public string Name { get; set; }
        }
        
        public override bool Execute(Input input)
        {
            var make = new Maker(_solution);
            
            Console2.BreakLine();

            make.Crud(input.In, input.Name);
            
            Console2.BreakLine();
            
            return true;
        }
    }
}