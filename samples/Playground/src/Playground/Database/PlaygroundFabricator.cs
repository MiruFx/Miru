using Miru.Fabrication;
using Miru.Fabrication.FixtureConventions;
using Playground.Features.Tables;

namespace Playground.Database
{
    public class PlaygroundFabricator : Fabricator
    {
        public PlaygroundFabricator(FabSupport context) : base(context)
        {
            Fixture.AddConvention(cfg =>
            {
                cfg.IfPropertyNameIs("Name").Use(f => f.Name.FullName());
            });
            
            WithDefault<TableList.Product>(x =>
            {
                x.ProductName = Faker.Commerce.ProductName();
                x.ProductPrice = Faker.Random.Decimal(0.5m, 99.99m);
                x.ProductLastSale = Faker.Date.Past(1);
            });
        }
    }
}
