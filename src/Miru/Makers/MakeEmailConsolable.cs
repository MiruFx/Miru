using Miru.Consolables;
using Miru.Core;
using Oakton;

namespace Miru.Makers
{
    [Description("Make a new Mailable", Name = "make:mail")]
    public class MakeEmailConsolable : ConsolableSync<MakeEmailConsolable.Input>
    {
        private readonly MiruSolution _solution;

        public MakeEmailConsolable(MiruSolution solution)
        {
            _solution = solution;
        }

        public class Input
        {
            public string In { get; set; }

            public string Name { get; set; }
            
            public string Action { get; set; }
        }
        
        public override bool Execute(Input input)
        {
            var make = new Maker(_solution);
            
            Console2.BreakLine();

            make.Email(input.In, input.Name, input.Action);
            
            return true;
        }
    }
}