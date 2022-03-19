using Corpo.Skeleton.Features.Teams;

namespace Corpo.Skeleton.PageTests.Pages.Teams;

public class TeamEditPageTest : PageTest
{
    [Test]
    public void Can_edit_team()
    {
        var team = _.MakeSaving<Team>();
            
        _.Visit(new TeamEdit.Query { Id = team.Id });

        _.Form<TeamEdit.Command>((f, command) =>
        {
            f.Input(m => m.Name, command.Name);
                
            f.Submit();
        });
            
        _.ShouldRedirectTo<TeamList>();
    }
}