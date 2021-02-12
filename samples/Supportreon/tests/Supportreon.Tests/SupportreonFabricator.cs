using System;
using Miru.Fabrication;
using Miru.Fabrication.FixtureConventions;
using Supportreon.Domain;

namespace Supportreon.Tests
{
    public class SupportreonFabricator : Fabricator
    {
        public SupportreonFabricator(FabSupport context) : base(context)
        {
            Fixture.AddConvention(conv =>
            {
                conv.IfPropertyNameIs(nameof(Project.Description))
                    .Use(f => f.Lorem.Paragraphs(1));
                
                conv.IfPropertyIs<Project>(p => p.EndDate)
                    .Use(f => f.Date.Future());

                conv.IfPropertyNameIs(nameof(Project.MinimumDonation))
                    .Use(f => f.Finance.Amount(min: Donation.Minimum, max: 7));

                conv.IfPropertyNameIs(nameof(Donation.Amount))
                    .Use(f => f.Finance.Amount(min: 7, max: 50));
                
                conv.IfPropertyNameIs(nameof(Donation.CreditCard))
                    .Use(f => f.Random.String2(10, "1234567890"));
                
                conv.IfPropertyNameIs(nameof(Project.CreatedAt))
                    .Use(f => DateTime.Now);
            });
        }
    }
}
