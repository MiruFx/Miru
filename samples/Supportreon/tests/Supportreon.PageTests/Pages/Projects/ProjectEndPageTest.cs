using Miru.PageTesting;
using Miru.Testing;
using NUnit.Framework;
using Supportreon.Domain;
using Supportreon.Features.Projects;

namespace Supportreon.PageTests.Pages.Projects
{
    public class ProjectEndPageTest : PageTest
    {
        [Test]
        public void Can_end_project()
        {
            var project = _.MakeSaving<Project>();
            
            _.Visit(new ProjectEnd.Query { Id = project.Id });

            _.ShouldHaveText($"Do you confirm ending the project {project.Name}");
            
            _.Form<ProjectEnd.Command>((f, command) =>
            {
                f.Submit();
            });
            
            _.ShouldHaveText("Project successfully ended");
        }
    }
}
