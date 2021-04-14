using Miru.Consolables;
using Miru.Core;
using Oakton;

namespace Miru.Makers
{
    [Description("Make a new appSettings", Name = "make:settings")]
    public class MakeAppSettingsConsolable : ConsolableSync<MakeAppSettingsConsolable.Input>
    {
        public class Input
        {
            [FlagAlias("e")]
            public string Environment { get; set; }
        }
        
        private readonly MiruSolution _solution;

        public MakeAppSettingsConsolable(MiruSolution solution)
        {
            _solution = solution;
        }

        public override bool Execute(Input input)
        {
            var maker = new Maker(_solution);
            
            maker.AppSettings(input.Environment);

            return true;
        }
    }
}