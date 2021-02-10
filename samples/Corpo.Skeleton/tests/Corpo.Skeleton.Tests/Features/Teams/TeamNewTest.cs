using System.Linq;
using System.Threading.Tasks;
using Corpo.Skeleton.Features.Teams;
using Miru.Testing;
using NUnit.Framework;
using Shouldly;

namespace Corpo.Skeleton.Tests.Features.Teams
{
    public class TeamNewTest : FeatureTest
    {
        [Test]
        public async Task Can_make_new_team()
        {
            // arrange
            var command = _.Make<TeamNew.Command>();
            
            // act
            var result = await _.SendAsync(command);
            
            // assert
            var saved = _.Db(db => db.Teams.First());
            saved.Name.ShouldBe(command.Name);
        }
        
        public class Validations : ValidationTest<TeamNew.Command>
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