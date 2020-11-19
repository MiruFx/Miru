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
    public class GoalNewTest : FeatureTest, IRequiresAuthenticatedUser
    {
        [Test]
        public async Task Can_add_new_goal()
        {
            // arrange
            var area = _.MakeSaving<Area>();
            var command = _.Make<GoalNew.Command>(m => m.AreaId = area.Id);

            // act
            var result = await _.SendAsync(command);

            // assert
            var saved = _.Db(db => db.Goals.First());
            saved.Name.ShouldBe(command.Name);
            saved.AreaId.ShouldBe(area.Id);
            saved.UserId.ShouldBe(_.CurrentUserId());
            saved.CreatedAt.ShouldBeSecondsAgo();
            saved.UpdatedAt.ShouldBeSecondsAgo();
            saved.IsInactive.ShouldBeFalse();
        }

        public class Validations : ValidationTest<GoalNew.Command>
        {
            [Test]
            public void Name_is_required()
            {
                ShouldBeValid(m => m.Name, Request.Name);
            
                ShouldBeInvalid(m => m.Name, string.Empty);
            }
            
            [Test]
            public void Area_id_is_required()
            {
                ShouldBeValid(m => m.AreaId, 10);
            
                ShouldBeInvalid(m => m.AreaId, 0);
            }
        }
    }
}
