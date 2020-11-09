using Miru.Consolables;
using Miru.Core;
using Oakton;

namespace Miru.Makers
{
    [Description("Make a Consolable", Name = "make:consolable")]
    public class MakeConsolableConsolable : ConsolableSync<MakeConsolableConsolable.Input>
    {
        private readonly Maker _maker;

        public class Input
        {
            public string Name { get; set; }
        }
        
        public MakeConsolableConsolable(Maker maker)
        {
            _maker = maker;
        }

        public override bool Execute(Input input)
        {
            _maker.Consolable(input.Name);

            return true;
        }
    }
}