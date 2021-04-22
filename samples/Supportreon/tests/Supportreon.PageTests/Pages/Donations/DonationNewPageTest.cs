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
            // arrange
            var project = _.MakeSaving<Project>();
            
            // act
            _.Visit(new DonationNew.Query { ProjectId = project.Id });

            _.Form<DonationNew.Command>((f, command) =>
            {
                // check override label content
                f.ShouldHaveText("Donation Amount");
                
                f.Input(m => m.Amount, command.Amount);
                f.Input(m => m.CreditCard, command.CreditCard);
                
                f.Submit();
            });
            
            // assert
            _.ShouldHaveText($"Thank you for your donation to {project.Name}!");
        }
    }
}
