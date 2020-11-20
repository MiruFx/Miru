using System.Linq;
using System.Threading.Tasks;
using Miru.Testing;
using NUnit.Framework;
using Shouldly;
using Mong.Domain;
using Mong.Features.Teams;

namespace Mong.Tests.Features.Teams
{
    public class TeamDisableTest : FeatureTest
    {
        [Test]
        public async Task Can_disable_team()
        {
            // arrange
            var team = _.MakeSaving<Team>();
            var command = _.Make<TeamDisable.Command>(m => m.Id = team.Id);

            // act
            var result = await _.SendAsync(command);

            // assert
            var saved = _.Db(db => db.Teams.First());
            saved.Name.ShouldBe(command.Name);
        }

        public class Validations : ValidationTest<TeamDisable.Command>
        {
            [Test]
            public void Name_is_required()
            {
                ShouldBeValid(m => m.Name, Request.Name);
            
                ShouldBeInvalid(m => m.Name, string.Empty);
            }
        }
    }
}
