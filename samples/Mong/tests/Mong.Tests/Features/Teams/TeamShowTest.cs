using System.Linq;
using System.Threading.Tasks;
using Miru.Testing;
using NUnit.Framework;
using Shouldly;
using Mong.Domain;
using Mong.Features.Teams;

namespace Mong.Tests.Features.Teams
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
