using System.Threading.Tasks;
using Miru.Testing;
using NUnit.Framework;
using Supportreon.Features.Projects;
using Shouldly;
using System.Linq;
using Supportreon.Domain;

namespace Supportreon.Tests.Features.Projects
{
    public class ProjectEndTest : FeatureTest
    {
        [Test]
        public async Task Can_end_project()
        {
            // arrange
            var project = _.MakeSaving<Project>();
            var command = _.Make<ProjectEnd.Command>(m => m.Id = project.Id);

            // act
            var result = await _.SendAsync(command);

            // assert
            var saved = _.Db(db => db.Projects.First());
            saved.IsActive.ShouldBeFalse();
        }

        public class Validations : ValidationTest<ProjectEnd.Command>
        {
            [Test]
            public void Id_is_required()
            {
                ShouldBeValid(m => m.Id, 1);
                
                ShouldBeInvalid(m => m.Id, 0);
            }
        }
    }
}
