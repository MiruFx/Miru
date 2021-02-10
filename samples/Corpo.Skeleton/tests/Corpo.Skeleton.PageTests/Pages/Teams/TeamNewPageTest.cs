using Corpo.Skeleton.Features.Teams;
using Miru.PageTesting;
using NUnit.Framework;

namespace Corpo.Skeleton.PageTests.Pages.Teams
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