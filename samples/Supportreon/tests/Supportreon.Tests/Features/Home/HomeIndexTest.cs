using System.Linq;
using System.Threading.Tasks;
using Miru.Testing;
using NUnit.Framework;
using Supportreon.Domain;
using Supportreon.Features.Home;

namespace Supportreon.Tests.Features.Home
{
    public class HomeIndexTest : FeatureTest
    {
        [Test]
        public async Task Show_new_projects()
        {
            // arrange
            var oldProjects = _.MakeMany<Project>(5);
            var newProjects = _.MakeMany<Project>(5);

            await _.SaveAsync(oldProjects, newProjects);

            // act
            var result = await _.SendAsync(new HomeIndex.Query());

            // assert
            result.Projects.ShouldContain(newProjects.ToArray()); // TODO: no need to convert to array
        }
    }
}
