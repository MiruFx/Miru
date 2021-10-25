namespace Corpo.Skeleton.Tests.Features.Teams;

public class TeamListTest : FeatureTest
{
    [Test]
    public async Task Can_list_teams()
    {
        // arrange
        var teams = _.MakeManySaving<Team>();
            
        // act
        var result = await _.SendAsync(new TeamList.Query());
            
        // assert
        result.Teams.ShouldCount(teams.Count());
    }
}