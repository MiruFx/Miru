using System.Threading.Tasks;
using Miru.PageTesting;
using Miru.Testing;
using Miru.Testing.Userfy;
using NUnit.Framework;
using SelfImprov.Domain;
using SelfImprov.Features.Goals;
using SelfImprov.Features.Iterations;
using SelfImprov.PageTests.Pages.Goals;

namespace SelfImprov.PageTests.Pages.Iterations
{
    public class IterationReviewPageTest : PageTest
    {
        private GoalsFixture _fix;

        public override async Task Given()
        {
            _fix = await _.Scenario<GoalsFixture>();
            
            _.Visit(new IterationReview());
        }
        
        [Test]
        public void Can_review_iteration()
        {
            _.Form<IterationReview.Command>((f, command) =>
            {
                f.Check(m => m.Areas[0].Goals[0].Achieved);
                f.Submit();
            });
            
            _.ShouldHaveText("Iteration successfully saved");
        }
    }
}
