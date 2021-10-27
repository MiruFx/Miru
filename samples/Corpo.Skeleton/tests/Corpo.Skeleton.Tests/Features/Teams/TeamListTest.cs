using Corpo.Skeleton.Features.Teams;

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
        
        result.Teams.First().Name.ShouldBe(teams.First().Name);
        result.Teams.Last().Name.ShouldBe(teams.Last().Name);
    }
}