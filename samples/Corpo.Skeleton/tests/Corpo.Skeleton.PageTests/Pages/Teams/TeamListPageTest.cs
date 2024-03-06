using Corpo.Skeleton.Features.Teams;

namespace Corpo.Skeleton.PageTests.Pages.Teams;

public class TeamListPageTest : PageTest
{
    [Test]
    public void Can_list_teams()
    {
        var teams = _.MakeMany<Team>();
        _.Save(teams);
            
        _.Visit<TeamList>();
            
        _.ShouldHaveText("Teams");

        _.Display<TeamList.Result>(x =>
        {
            x.ShouldHave(m => m.Teams[0].Name, teams.At(0).Name);
        });
    }
}