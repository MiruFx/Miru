using Miru.Consolables;
using Miru.Core;
using Oakton;

namespace Miru.Makers
{
    [Description("Make a Config.yml file for an environment", Name = "make:config")]
    public class MakeConfigConsolable : ConsolableSync<MakeConfigConsolable.Input>
    {
        public class Input
        {
            [FlagAlias("e")]
            public string Environment { get; set; }
        }
        
        private readonly MiruSolution _solution;

        public MakeConfigConsolable(MiruSolution solution)
        {
            _solution = solution;
        }

        public override bool Execute(Input input)
        {
            var maker = new Maker(_solution);
            
            maker.ConfigYml(input.Environment);

            return true;
        }
    }
}