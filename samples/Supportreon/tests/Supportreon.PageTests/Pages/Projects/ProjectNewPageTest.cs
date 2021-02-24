using Miru.PageTesting;
using Miru.Testing;
using Miru.Testing.Userfy;
using NUnit.Framework;
using Supportreon.Domain;
using Supportreon.Features.Projects;

namespace Supportreon.PageTests.Pages.Projects
{
    public class ProjectNewPageTest : PageTest, IRequiresAuthenticatedUser
    {
        [Test]
        public void Can_create_new_project()
        {
            var category = _.MakeSaving<Category>();
            
            _.Visit<ProjectNew>();
            
            _.Form<ProjectEdit.Command>((f, command) =>
            {    
                f.Input(m => m.Name, command.Name);
                f.Select(m => m.CategoryId, category.Name);
                f.Input(m => m.Description, command.Description);
                f.Input(m => m.MinimumDonation, command.MinimumDonation);
                f.Input(m => m.Goal, command.Goal);
                f.Input(m => m.EndDate, _.Faker().Date.Future());
                
                f.Submit();
            });
            
            _.ShouldHaveText("Your Project has been saved");
        }
    }
}
