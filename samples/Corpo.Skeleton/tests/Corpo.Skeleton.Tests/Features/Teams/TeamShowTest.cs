using System.Threading.Tasks;
using Corpo.Skeleton.Domain;
using Corpo.Skeleton.Features.Teams;
using Miru.Testing;
using NUnit.Framework;
using Shouldly;

namespace Corpo.Skeleton.Tests.Features.Teams
{
    public class TeamShowTest : FeatureTest
    {
        [Test]
        public async Task Can_show_team()
        {
            // arrange
            var team = _.MakeSaving<Team>();
            
            // act
            var response = await _.SendAsync(new TeamShow.Query { Id = team.Id });
            
            // assert
            response.Team.ShouldBe(team);
        }
    }
}