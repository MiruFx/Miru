using Miru.Fabrication;
using Miru.Fabrication.FixtureConventions;

namespace MiruNext.Database;

public class AppFabricator : Fabricator
{
    public AppFabricator(FabSupport context) : base(context)
    {
        Fixture.AddConvention(cfg =>
        {
            cfg.IfPropertyNameIs("Name").Use(f => f.Name.FullName());
        });
    }
}