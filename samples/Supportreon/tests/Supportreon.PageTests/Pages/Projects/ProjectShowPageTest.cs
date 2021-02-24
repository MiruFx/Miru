using Miru.PageTesting;
using Miru.Testing;
using NUnit.Framework;
using Supportreon.Domain;
using Supportreon.Features.Projects;

namespace Supportreon.PageTests.Pages.Projects
{
    public class ProjectShowPageTest : PageTest
    {
        [Test]
        public void Can_Show_Project()
        {
            var project = _.MakeSaving<Project>();

            _.Visit(new ProjectShow.Query { Id = project.Id });
            
            _.ShouldHaveText(project.Name);
            _.ShouldHaveText(project.Description);
        }
    }
}
