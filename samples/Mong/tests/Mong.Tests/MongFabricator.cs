using System;
using Miru.Fabrication;
using Miru.Fabrication.FixtureConventions;
using Mong.Domain;

namespace Mong.Tests
{
    public class MongFabricator : Fabricator
    {
        public UserFabricator Users => FabFor<User, UserFabricator>();

        public MongFabricator(FabSupport context) : base(context)
        {
            Fixture.AddConvention(cfg =>
            {
                cfg.IfPropertyNameIs("Amount").Use(f => decimal.Round(f.Random.Decimal(5, 100), 2));

                cfg.IfPropertyNameIs("TransactionId").Use(() => Guid.NewGuid().ToString());
                cfg.IfPropertyNameIs("PaymentId").Use(() => Guid.NewGuid().ToString());
                
                cfg.IfPropertyNameIs("BlockedAt").Use(() => null);
            });
        }
        
        public class UserFabricator : CustomFabricator<User, UserFabricator>
        {
            public UserFabricator(FabSupport support) : base(support) { }
            
            public UserFabricator Admin() => With(m =>
            {
                m.IsAdmin = true;
                m.Email = "admin@admin.com";
                m.Name = "Admin";
            });
            
            public UserFabricator Customer() => With(m =>
            {
                m.IsAdmin = false;
            });
        }
    }
}