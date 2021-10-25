using System.Linq;
using System.Threading.Tasks;
using Corpo.Skeleton.Domain;
using Corpo.Skeleton.Features.Teams;
using Miru.Testing;
using NUnit.Framework;

namespace Corpo.Skeleton.Tests.Features.Teams
{
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
}