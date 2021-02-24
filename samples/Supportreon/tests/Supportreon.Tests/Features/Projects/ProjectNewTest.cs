using System.Threading.Tasks;
using Miru.Testing;
using Miru.Testing.Userfy;
using NUnit.Framework;
using Supportreon.Features.Projects;
using Supportreon.Domain;

namespace Supportreon.Tests.Features.Projects
{
    public class ProjectNewTest : FeatureTest, IRequiresAuthenticatedUser
    {
        [Test]
        public async Task Can_query_for_new_project()
        {
            // arrange
            _.MakeSaving<Category>();
            
            var query = _.Make<ProjectNew.Query>();

            // act
            var command = await _.SendAsync(query);

            // assert
            command.Categories.ShouldCount(1);
        }
        
        public class Authorizations : AuthorizationTest
        {
            [Test]
            public async Task Should_be_authenticated()
            {
                var user = _.MakeSaving<User>();

                await _.ShouldAuthorize(user, new ProjectNew.Query());

                await _.ShouldNotAuthorizeAnonymous(new ProjectNew.Query());
            }
        }
    }
}
