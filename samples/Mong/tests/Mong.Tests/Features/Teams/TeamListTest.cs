using System.Linq;
using System.Threading.Tasks;
using Miru.Testing;
using NUnit.Framework;
using Mong.Domain;
using Mong.Features.Teams;

namespace Mong.Tests.Features.Teams
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
            result.Items.ShouldCount(teams.Count());
        }
    }
}
