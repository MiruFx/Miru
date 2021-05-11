using Miru.Fabrication;
using Miru.Fabrication.FixtureConventions;

namespace Pantanal.Tests
{
    public class PantanalFabricator : Fabricator
    {
        public PantanalFabricator(FabSupport context) : base(context)
        {
            Fixture.AddConvention(cfg =>
            {
                cfg.IfPropertyNameIs("Name").Use(f => f.Name.FullName());
            });
        }
    }
}
