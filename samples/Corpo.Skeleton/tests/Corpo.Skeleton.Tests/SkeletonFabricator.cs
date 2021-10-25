using Miru.Fabrication;
using Miru.Fabrication.FixtureConventions;

namespace Corpo.Skeleton.Tests;

public class SkeletonFabricator : Fabricator
{
    public SkeletonFabricator(FabSupport context) : base(context)
    {
        Fixture.AddConvention(cfg =>
        {
            cfg.IfPropertyNameIs("Name").Use(f => f.Name.FullName());
        });
    }
}