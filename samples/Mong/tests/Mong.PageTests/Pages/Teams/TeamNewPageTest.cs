using Miru.PageTesting;
using Miru.Testing;
using NUnit.Framework;
using Mong.Domain;
using Mong.Features.Teams;

namespace Mong.PageTests.Pages.Teams
{
    public class TeamNewPageTest : PageTest
    {
        [Test]
        public void Can_make_new_team()
        {
            _.Visit(new TeamNew());

            _.Form<TeamNew.Command>((f, command) =>
            {
                f.Input(m => m.Name, command.Name);
                
                f.Submit();
            });
            
            _.ShouldHaveText("Team successfully saved");
        }
    }
}
