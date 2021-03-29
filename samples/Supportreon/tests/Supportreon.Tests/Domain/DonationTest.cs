using Miru.Domain;
using Miru.Testing;
using NUnit.Framework;
using Shouldly;
using Supportreon.Domain;
using Supportreon.Features.Donations;

namespace Supportreon.Tests.Domain
{
    public class DonationTest : DomainTest
    {
        [Test]
        public void Make_new_donation()
        {
            // arrange
            var project = _.Make<Project>(m =>
            {
                m.TotalDonations = 10;
                m.TotalAmount = 200;
            });
            
            var donor = _.Make<User>();
            
            var request = _.Make<DonationNew.Command>(m =>
            {
                m.Amount = project.MinimumDonation + 1;
                m.ProjectId = project.Id;
            });
            
            // act
            var donation = new Donation(request, project, donor);
            
            // assert
            donation.Amount.ShouldBe(request.Amount);
            donation.CreditCard.ShouldBe(request.CreditCard);
            donation.User.ShouldBe(donor);

            project.TotalDonations.ShouldBe(11);
            project.TotalAmount.ShouldBe(request.Amount + 200);
        }

        [Test]
        public void Cannot_make_new_donation_for_ended_project()
        {
            // arrange
            var project = _.Make<Project>(m => m.EndDate = _.Faker().Date.Past());
            var donor = _.Make<User>();
            
            var request = _.Make<DonationNew.Command>(m =>
            {
                m.Amount = project.MinimumDonation + 1;
                m.ProjectId = project.Id;
            });
            
            // act
            Should.Throw<DomainException>(() => new Donation(request, project, donor));
        }
        
        [Test]
        public void Cannot_make_donation_amount_less_than_minimum_for_the_project()
        {
            // arrange
            var project = _.Make<Project>();
            var donor = _.Make<User>();
            
            var request = _.Make<DonationNew.Command>(m =>
            {
                m.Amount = project.MinimumDonation - 1;
                m.ProjectId = project.Id;
            });
            
            // act
            Should.Throw<DomainException>(() => new Donation(request, project, donor));
        }
    }
}
