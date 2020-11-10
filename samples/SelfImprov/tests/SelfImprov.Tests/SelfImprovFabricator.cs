using System;
using Miru;
using Miru.Domain;
using Miru.Fabrication;
using Miru.Fabrication.FixtureConventions;
using Miru.Userfy;

namespace SelfImprov.Tests
{
    public class SelfImprovFabricator : Fabricator
    {
        public SelfImprovFabricator(FabSupport context) : base(context)
        {
            Fixture.AddConvention(_ =>
            {
                _.IfPropertyNameIs("Name").Use(f => f.Name.FullName());
                
                _.IfPropertyImplements<IUser>().Ignore();

                // TODO: Support IfPropertyIs<IInactivable>(m => m.IsInactive)
                _.IfPropertyNameIs(nameof(IInactivable.IsInactive)).Use(false);
                
                _.IfPropertyNameStarts("Password").Use("123456");
                _.IfPropertyNameIs("HashedPassword").Use(() => Hash.Create("123456"));
            });
        }
    }
}
