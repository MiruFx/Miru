using Miru.PageTesting;
using Miru.Testing;
using Miru.Testing.Userfy;
using NUnit.Framework;
using Supportreon.Domain;
using Supportreon.Features.Donations;

namespace Supportreon.PageTests.Pages.Donations
{
    public class DonationNewPageTest : PageTest, IRequiresAuthenticatedUser
    {
        [Test]
        public void New_donation()
        {
            var project = _.MakeSaving<Project>();
            
            _.Visit(new DonationNew.Query { ProjectId = project.Id });

            _.Form<DonationNew.Command>((f, command) =>
            {
                f.Input(m => m.Amount, command.Amount);
                f.Input(m => m.CreditCard, command.CreditCard);
                
                f.Submit();
            });
            
            _.ShouldHaveText($"Thank you for your donation to {project.Name}!");
        }
    }
}
