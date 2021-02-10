using Corpo.Skeleton.Domain;
using Corpo.Skeleton.Features.Teams;
using Miru;
using Miru.PageTesting;
using Miru.Testing;
using NUnit.Framework;

namespace Corpo.Skeleton.PageTests.Pages.Teams
{
    public class TeamListPageTest : PageTest
    {
        [Test]
        public void Can_list_teams()
        {
            var teams = _.MakeManySaving<Team>();
            
            _.Visit<TeamList>();
            
            _.ShouldHaveText("Teams");

            _.Display<TeamList.Result>(x =>
            {
                x.ShouldHave(m => m.Items[0].Name, teams.At(0).Name);
            });
        }
    }
}