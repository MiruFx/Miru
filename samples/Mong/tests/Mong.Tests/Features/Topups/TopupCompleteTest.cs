using System;
using Miru.Testing;
using NUnit.Framework;
using Shouldly;
using Mong.Features.Topups;
using System.Linq;
using System.Threading.Tasks;
using Mong.Domain;
using Mong.Payments;
using NSubstitute;

namespace Mong.Tests.Features.Topups
{
    public class TopupCompleteTest : OneCaseFeatureTest
    {
        private TopupComplete _command;
        private Topup _topup;

        public override async Task Given()
        {
            // arrange
            _topup = _.MakeSaving<Topup>(m => m.Status = TopupStatus.Paid);
            _command = _.Make<TopupComplete>(m => m.TopupId = _topup.Id);

            // act
            await _.Send(_command);
        }

        [Test]
        public void Mobile_provider_added_credit()
        {
            _.Get<IMobileProvider>().Received().Topup(_topup.PhoneNumber, _topup.Amount);
        }

        [Test]
        public void Topup_status_completed()
        {
            var saved = _.Db(db => db.Topups.First());
            
            saved.Status.ShouldBe(TopupStatus.Completed);
        }

        [Test]
        public void Email_sent()
        {
            var email = _.LastEmailSent();
            email.Data.ToAddresses[0].EmailAddress.ShouldBe(_topup.Email);
            email.Data.ToAddresses[0].Name.ShouldBe(_topup.Name);
            email.Data.Subject.ShouldBe("Topup Successful");
            email.Data.Body.ShouldContain(_topup.PhoneNumber);
        }

        public class Errors : FeatureTest
        {
            [Test]
            public async Task If_mobile_provider_fails_then_notify_user_by_email()
            {
                // arrange
                var user = _.Make<User>();
                var topup = _.Make<Topup>(m =>
                {
                    m.User = user;
                    m.Status = TopupStatus.Paid;
                    m.Email = user.Email;
                    m.Name = user.Name;
                });

                await _.Save(user, topup);
                
                var command = _.Make<TopupComplete>(m => m.TopupId = topup.Id);
                
                _.Get<IMobileProvider>()
                    .When(m => m.Topup(topup.PhoneNumber, topup.Amount))
                    .Do(m => throw new Exception("Mobile provider failed to topup phone number"));
                
                // act
                await _.Send(command);
                
                // assert
                var saved = _.Db(db => db.Topups.First());
                saved.Status.ShouldBe(TopupStatus.Error);
                
                var email = _.LastEmailSent();
                email.Data.ToAddresses[0].EmailAddress.ShouldBe(user.Email);
                email.Data.Subject.ShouldBe("Topup Failed");
                email.Data.Body.ShouldContain(topup.PhoneNumber);
            }
        }
    }
}