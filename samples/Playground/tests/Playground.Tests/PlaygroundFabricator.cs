using Miru.Fabrication;
using Miru.Fabrication.FixtureConventions;

namespace Playground.Tests
{
    public class PlaygroundFabricator : Fabricator
    {
        public PlaygroundFabricator(FabSupport context) : base(context)
        {
            Fixture.AddConvention(cfg =>
            {
                cfg.IfPropertyNameIs("Name").Use(f => f.Name.FullName());
            });
        }
    }
}
