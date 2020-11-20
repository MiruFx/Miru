using Miru.PageTesting;
using Miru.Testing;
using NUnit.Framework;
using Mong.Domain;
using Mong.Features.Teams;

namespace Mong.PageTests.Pages.Teams
{
    public class TeamDisablePageTest : PageTest
    {
        [Test]
        public void Can_disable_team()
        {
            var team = _.MakeSaving<Team>();
            
            _.Visit(new TeamDisable.Query { Id = team.Id });

            _.Form<TeamDisable.Command>((f, command) =>
            {
                f.Input(m => m.Name, command.Name);
                
                f.Submit();
            });
            
            _.ShouldHaveText("Team successfully saved");
        }
    }
}
