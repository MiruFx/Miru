using System.Threading.Tasks;
using Miru.Testing;
using NUnit.Framework;
using Supportreon.Features.Donations;
using Shouldly;
using System.Linq;
using Miru;
using Miru.Domain;
using Miru.Testing.Userfy;
using Supportreon.Domain;

namespace Supportreon.Tests.Features.Donations
{
    public class DonationNewTest : FeatureTest, IRequiresAuthenticatedUser
    {
        [Test]
        public async Task Can_query_for_new_donation()
        {
            // arrange
            var project = _.MakeSaving<Project>();
            
            // act
            var command = await _.SendAsync(new DonationNew.Query
            {
                ProjectId = project.Id
            });

            // assert
            command.Amount.ShouldBe(project.MinimumDonation);
            command.Project.ShouldBe(project);
            command.ProjectId.ShouldBe(project.Id);
        }
        
        [Test]
        public async Task Can_make_new_donation()
        {
            // arrange
            var project = _.MakeSaving<Project>();
            
            // act
            var command = _.Make<DonationNew.Command>(m =>
            {
                m.ProjectId = project.Id;
                m.Amount = project.MinimumDonation + 1;
            });
            
            var result = await _.SendAsync(command);

            // assert
            result.Project.ShouldBe(project);
            
            var savedDonation = _.Db(db => db.Donations.First());
            savedDonation.ProjectId.ShouldBe(command.ProjectId);
            savedDonation.Amount.ShouldBe(command.Amount);
            savedDonation.CreditCard.ShouldBe(command.CreditCard);
            savedDonation.UserId.ShouldBe(_.CurrentUserId());

            var savedProject = _.Db(db => db.Projects.First());
            savedProject.TotalAmount.ShouldBe(project.TotalAmount + command.Amount);
            savedProject.TotalDonations.ShouldBe(project.TotalDonations + 1);

            var email = _.EnqueuedEmail();
            email.Subject.ShouldBe("Thank you!");
            email.ToAddresses.ShouldContain(TestingCurrentUser.User.Email);
            email.Body.ShouldContain(project.Name);
        }
        
        [Test]
        public async Task Cannot_make_donation_amount_less_than_minimum_for_the_project()
        {
            // arrange
            var owner = _.Make<User>();
            var project = _.Make<Project>(m => m.User = owner);
            
            await _.SaveAsync(owner, project);
            
            // act
            var command = _.Make<DonationNew.Command>(m =>
            {
                m.Amount = project.MinimumDonation - 1;
                m.ProjectId = project.Id;
            });
            
            Should.Throw<DomainException>(() => _.SendSync(command));
        }

        public class Validations : ValidationTest<DonationNew.Command>
        {
            [Test]
            public void ProjectId_is_required()
            {
                ShouldBeValid(m => m.ProjectId, 1);
            
                ShouldBeInvalid(m => m.ProjectId, 0);
            }
            
            [Test]
            public void Amount_is_required()
            {
                ShouldBeValid(m => m.Amount, Request.Amount);
            
                ShouldBeInvalid(m => m.Amount, 0);
            }
            
            [Test]
            public void Credit_card_is_required()
            {
                ShouldBeValid(m => m.CreditCard, Request.CreditCard);
            
                ShouldBeInvalid(m => m.CreditCard, string.Empty);
            }
        }
        
        public class Authorizations : AuthorizationTest
        {
            [Test]
            public async Task Should_be_authenticated()
            {
                var project = _.MakeSaving<Project>();
                
                await _.ShouldNotAuthorize(new DonationNew.Query
                {
                    ProjectId = project.Id
                });
                
                await _.ShouldNotAuthorize(new DonationNew.Command());
            }
        }
    }
}
