using System.Threading.Tasks;
using Miru.Testing;
using NUnit.Framework;
using SelfImprov.Features.Goals;
using Shouldly;
using System.Linq;
using Miru.Testing.Userfy;
using SelfImprov.Domain;

namespace SelfImprov.Tests.Features.Goals
{
    public class GoalEditTest : FeatureTest, IRequiresAuthenticatedUser
    {
        [Test]
        public async Task Can_edit_goal()
        {
            // arrange
            var goal = _.MakeSaving<Goal>();
            var command = _.Make<GoalEdit.Command>(m => m.Id = goal.Id);

            // act
            var result = await _.Send(command);

            // assert
            var saved = _.Db(db => db.Goals.First());
            saved.Name.ShouldBe(command.Name);
        }

        public class Validations : ValidationTest<GoalEdit.Command>
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
