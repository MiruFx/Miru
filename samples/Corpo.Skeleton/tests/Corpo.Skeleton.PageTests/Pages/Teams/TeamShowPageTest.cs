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
        public void Can_show_team()
        {
            var team = _.MakeSaving<Team>();
            
            _.Visit(new TeamShow.Query { Id = team.Id });

            _.ShouldHaveText(team.Name);
        }
    }
}