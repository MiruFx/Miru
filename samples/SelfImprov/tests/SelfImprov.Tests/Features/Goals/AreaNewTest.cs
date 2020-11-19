using System.Linq;
using System.Threading.Tasks;
using Miru.Testing;
using Miru.Testing.Userfy;
using NUnit.Framework;
using SelfImprov.Domain;
using SelfImprov.Features.Goals;
using Shouldly;

namespace SelfImprov.Tests.Features.Goals
{
    public class AreaNewTest : FeatureTest, IRequiresAuthenticatedUser
    {
        [Test]
        public async Task Can_make_new_area()
        {
            // arrange
            var command = _.Make<AreaNew.Command>();

            // act
            await _.SendAsync(command);

            // assert
            var saved = _.Db(db => db.Areas.First());
            saved.Name.ShouldBe(command.Name);
            saved.UserId.ShouldBe(_.CurrentUserId());
            saved.CreatedAt.ShouldBeSecondsAgo();
            saved.UpdatedAt.ShouldBeSecondsAgo();
        }

        public class Validations : ValidationTest<AreaNew.Command>, IRequiresAuthenticatedUser
        {
            [Test]
            public void Name_is_required_and_unique_per_user()
            {
                var otherUser = _.MakeSaving<User>();
                var existentOtherUser = _.MakeSaving<Area>(m => m.UserId = otherUser.Id);
                
                var existent = _.MakeSaving<Area>();

                ShouldBeValid(m => m.Name, Request.Name);
                ShouldBeValid(m => m.Name, existentOtherUser.Name);
                
                ShouldBeInvalid(m => m.Name, existent.Name);
                ShouldBeInvalid(m => m.Name, string.Empty);
            }
        }
    }
}
