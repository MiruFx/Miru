using System.Linq;
using System.Threading.Tasks;
using Corpo.Skeleton.Domain;
using Corpo.Skeleton.Features.Teams;
using Miru.Testing;
using NUnit.Framework;
using Shouldly;

namespace Corpo.Skeleton.Tests.Features.Teams
{
    public class TeamEditTest : FeatureTest
    {
        [Test]
        public async Task Can_edit_team()
        {
            // arrange
            var team = _.MakeSaving<Team>();
            var command = _.Make<TeamEdit.Command>(m => m.Id = team.Id);

            // act
            var result = await _.SendAsync(command);

            // assert
            var saved = _.Db(db => db.Teams.First());
            saved.Name.ShouldBe(command.Name);
        }

        public class Validations : ValidationTest<TeamEdit.Command>
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