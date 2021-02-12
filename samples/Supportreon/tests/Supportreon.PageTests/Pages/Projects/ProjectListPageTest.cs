using Miru;
using Miru.PageTesting;
using Miru.Testing;
using NUnit.Framework;
using Supportreon.Domain;
using Supportreon.Features.Projects;

namespace Supportreon.PageTests.Pages.Projects
{
    public class ProjectListPageTest : PageTest
    {
        [Test]
        public void Can_list_projects()
        {
            var projects = _.MakeMany<Project>();

            _.Save(projects);

            _.Visit<ProjectList.Query>();
            
            _.ShouldHaveText("Projects");

            _.ShouldHaveText(projects.At(2).Name);
                
            _.ShouldHaveText(projects.At(1).Name);

            _.ShouldHaveText(projects.At(0).Name);
            
            // _.Display<ProjectList.Query>(x =>
            // {
            //     // created at desc
            //     x.ShouldHave(m => m.Results[0].Name, projects.At(2).Name);
            //     
            //     x.ShouldHave(m => m.Results[1].Name, projects.At(1).Name);
            //
            //     x.ShouldHave(m => m.Results[2].Name, projects.At(0).Name);
            // });
        }
    }
}
