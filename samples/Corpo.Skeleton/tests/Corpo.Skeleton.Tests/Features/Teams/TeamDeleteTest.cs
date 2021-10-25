using System.Linq;
using System.Threading.Tasks;
using Corpo.Skeleton.Domain;
using Corpo.Skeleton.Features.Teams;
using Miru.Testing;
using NUnit.Framework;
using Shouldly;

namespace Corpo.Skeleton.Tests.Features.Teams
{
    public class TeamDeleteTest : FeatureTest
    {
        [Test]
        public async Task Should_delete_a_team()
        {
            // arrange
            var team = _.Make<Team>();

            await _.SaveAsync(team);
            
            // act
            await _.SendAsync(new TeamDelete.Command { Id = team.Id });

            // assert
            _.Db(db =>
            {
                db.Teams.Any().ShouldBeFalse();
            });
        }
    }
}