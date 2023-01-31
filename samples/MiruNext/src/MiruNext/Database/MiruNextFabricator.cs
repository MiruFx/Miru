using Miru.Fabrication;
using Miru.Fabrication.FixtureConventions;

namespace MiruNext.Database;

public class MiruNextFabricator : Fabricator
{
    public MiruNextFabricator(FabSupport context) : base(context)
    {
        Fixture.AddConvention(cfg =>
        {
            cfg.IfPropertyNameIs("Name").Use(f => f.Name.FullName());
        });
    }
}