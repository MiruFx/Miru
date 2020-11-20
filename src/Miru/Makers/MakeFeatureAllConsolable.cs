using Miru.Consolables;
using Miru.Core;
using Oakton;

namespace Miru.Makers
{
    [Description("Make new, edit, show, and list Features ", Name = "make:feature:all")]
    public class MakeFeatureAllConsolable : ConsolableSync<MakeFeatureAllConsolable.Input>
    {
        private readonly MiruSolution _solution;

        public MakeFeatureAllConsolable(MiruSolution solution)
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

            make.FeatureAll(input.In, input.Name);
            
            Console2.BreakLine();
            
            return true;
        }
    }
}