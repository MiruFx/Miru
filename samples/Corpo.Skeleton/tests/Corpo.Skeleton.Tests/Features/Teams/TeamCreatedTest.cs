namespace Corpo.Skeleton.Tests.Features.Teams;

public class TeamCreatedTest : FeatureTest
{
    [Test]
    public async Task Can_handle_created_team_job()
    {
        // arrange
        var command = _.Make<TeamCreated.Job>();
            
        // act
        await _.SendAsync(command);
            
        // assert
    }
}