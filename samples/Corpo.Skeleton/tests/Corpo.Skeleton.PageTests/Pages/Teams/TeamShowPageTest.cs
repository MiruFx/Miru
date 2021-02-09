using Corpo.Skeleton.Domain;
using Corpo.Skeleton.Features.Teams;
using Miru.PageTesting;
using Miru.Testing;
using NUnit.Framework;

namespace Corpo.Skeleton.PageTests.Pages.Teams
{
    public class TeamShowPageTest : PageTest
    {
        [Test]
        public void Can_show_product()
        {
            var product = _.MakeSaving<Team>();
            
            _.Visit(new TeamShow.Query { Id = product.Id });

            _.ShouldHaveText(product.Name);
        }
    }
}