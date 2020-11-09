using System.Threading.Tasks;
using Miru.Testing;
using NUnit.Framework;
using SelfImprov.Features.Goals;
using Shouldly;
using System.Linq;
using Miru.Testing.Userfy;
using SelfImprov.Domain;
using Z.EntityFramework.Plus;

namespace SelfImprov.Tests.Features.Goals
{
    public class GoalRemoveTest : FeatureTest, IRequiresAuthenticatedUser
    {
        [Test]
        public async Task Can_inactive_goal()
        {
            // arrange
            var goal = _.MakeSaving<Goal>();

            // act
            await _.Send(new GoalRemove.Command
            {
                Id = goal.Id
            });

            // assert
            var saved = _.Db(db => db.Goals.AsNoFilter().First());
            saved.IsInactive.ShouldBeTrue();
        }
    }
}
