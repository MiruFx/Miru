using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Miru.Domain;
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
        
        [Test]
        public async Task Area_name_should_be_unique_per_user()
        {
            // arrange
            var user = _.MakeSavingLogin<User>();
            var existentArea = _.MakeSaving<Area>(m => m.UserId = user.Id);

            await _.SaveAsync(user, existentArea);
            
            var command = _.Make<AreaNew.Command>(m => m.Name = existentArea.Name);

            // act
            await Should.ThrowAsync<DomainException>(() => _.SendAsync(command));
        }

        public class Validations : ValidationTest<AreaNew.Command>, IRequiresAuthenticatedUser
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
