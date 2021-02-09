using Corpo.Skeleton.Domain;
using Corpo.Skeleton.Features.Teams;
using Miru.PageTesting;
using Miru.Testing;
using NUnit.Framework;

namespace Corpo.Skeleton.PageTests.Pages.Teams
{
    public class TeamEditPageTest : PageTest
    {
        [Test]
        public void Can_edit_product()
        {
            var product = _.MakeSaving<Team>();
            
            _.Visit(new TeamEdit.Query { Id = product.Id });

            _.Form<TeamEdit.Command>((f, command) =>
            {
                f.Input(m => m.Name, command.Name);
                
                f.Submit();
            });
            
            _.ShouldHaveText("Product successfully saved");
        }
    }
}