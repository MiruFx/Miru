using Miru.Consolables;
using Miru.Core;
using Oakton;

namespace Miru.Makers
{
    [Description("Make a FluentMigrator Migration", Name = "make:migration")]
    public class MakeMigrationConsolable : ConsolableSync<MakeMigrationConsolable.Input>
    {
        private readonly Maker _maker;

        public class Input
        {
            public string Name { get; set; }
        }
        
        public MakeMigrationConsolable(Maker maker)
        {
            _maker = maker;
        }

        public override bool Execute(Input input)
        {
            _maker.Migration(input.Name);

            return true;
        }
    }
}