using System.Threading.Tasks;
using Miru.Testing;
using NUnit.Framework;
using Supportreon.Features.Projects;
using Shouldly;
using System.Linq;
using Miru.Domain;
using Supportreon.Domain;

namespace Supportreon.Tests.Features.Projects
{
    public class ProjectEditTest : FeatureTest
    {
        [Test]
        public async Task Can_edit_project()
        {
            // arrange
            var category = _.MakeSaving<Category>();
            var project = _.MakeSaving<Project>();
            var command = _.Make<ProjectEdit.Command>(m =>
            {
                m.Id = project.Id;
                m.CategoryId = category.Id;
            });

            // act
            await _.SendAsync(command);

            // assert
            var saved = _.Db(db => db.Projects.First());
            saved.Name.ShouldBe(command.Name);
        }
        
        [Test]
        public async Task Cannot_edit_project_using_existent_name()
        {
            // arrange
            var category = _.Make<Category>();
            var project = _.Make<Project>(m => m.Category = category);
            var otherProject = _.Make<Project>(m => m.Category = category);

            await _.SaveAsync(category, project, otherProject);
            
            var command = _.Make<ProjectEdit.Command>(m =>
            {
                m.Id = project.Id;
                m.CategoryId = category.Id;
                m.Name = otherProject.Name;
            });

            // act & assert
            await Should.ThrowAsync<DomainException>(() => _.SendAsync(command));
        }

        public class Validations : ValidationTest<ProjectEdit.Command>
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
