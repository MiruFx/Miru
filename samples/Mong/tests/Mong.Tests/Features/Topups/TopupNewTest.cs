using Miru.Testing;
using NUnit.Framework;
using Shouldly;
using Mong.Features.Topups;
using System.Linq;
using System.Threading.Tasks;
using Miru.Domain;
using Miru.Testing.Userfy;
using Mong.Domain;
using Mong.Payments;
using NSubstitute;

namespace Mong.Tests.Features.Topups
{
    // #featuretest
    public class TopupNewTest : OneCaseFeatureTest, IRequiresAuthenticatedUser
    {
        private TopupNew.Command _command;
        private PayPauResult _payment;

        public override async Task Given()
        {
            // arrange
            var provider = _.MakeSaving<Provider>(m => m.Amounts = "20");

            _payment = _.Make<PayPauResult>();
            _command = _.Make<TopupNew.Command>(m =>
            {
                m.ProviderId = provider.Id;
                m.Amount = 20;
            });
                
            _.Get<IPayPau>().Charge(_command.CreditCard, _command.Amount).Returns(_payment);

            // act
            await _.Send(_command);
        }

        [Test]
        public void Credit_card_charged_by_Paypau()
        {
            _payment.TransactionId.ShouldNotBeNull();
        }

        /// <summary>
        /// Check only some properties that matters for the operation
        /// All properties are tested at DomainTest
        /// </summary>
        [Test]
        public void New_topup_saved()
        {
            var saved = _.Db(db => db.Topups.First());
            
            saved.Status.ShouldBe(TopupStatus.Paid);
            saved.PaymentId.ShouldBe(_payment.TransactionId);
            saved.ProviderId.ShouldBe(_command.ProviderId);
            saved.PhoneNumber.ShouldBe(_command.PhoneNumber);
            saved.Amount.ShouldBe(_command.Amount);
            saved.UserId.ShouldBe(_.CurrentUserId());
            saved.Email.ShouldBe(_.CurrentUser<User>().Email);
            saved.CreatedAt.ShouldBeSecondsAgo();
            saved.UpdatedAt.ShouldBeSecondsAgo();
        }

        [Test]
        public void Topup_job_enqueued()
        {
            _.EnqueuedOneJobFor<TopupComplete>().ShouldBeTrue();
        }

        public class Errors : FeatureTest, IRequiresAuthenticatedUser
        {
            [Test]
            public async Task Do_not_save_topup_if_credit_card_charge_fails()
            {
                // arrange
                var provider = _.MakeSaving<Provider>();
                var command = _.Make<TopupNew.Command>(m => m.ProviderId = provider.Id);
                
                _.Get<IPayPau>()
                    .When(m => m.Charge(command.CreditCard, command.Amount))
                    .Do(m => throw new DomainException("Payment failed"));
                
                // act
                await Should.ThrowAsync<DomainException>(async () => await _.Send(command));
                
                // assert
                _.Db(db => db.Topups.ToList()).ShouldBeEmpty();
            }
        }
        
        public class Query : FeatureTest
        {
            [Test]
            public async Task Query_new_topup()
            {
                // arrange
                var providers = _.MakeMany<Provider>();
                var user = _.Make<User>();

                _.SaveSync(providers, user);
                _.LoginAs(user);
            
                // act
                var command = await _.Send(new TopupNew.Query());
            
                // assert
                command.Name.ShouldBe(user.Name);
                command.Email.ShouldBe(user.Email);
                command.Providers.Count().ShouldBe(3);
            }    
        }
        
        public class Validation : OneCaseFeatureTest
        {
            private TopupNew.Command _request;

            public override void GivenSync()
            {
                _request = _.Make<TopupNew.Command>();
            }

            [Test]
            public void Phone_is_required()
            {
                _.ShouldBeInvalid(_request, x => x.PhoneNumber, string.Empty);
                _.ShouldBeValid(_request, x => x.PhoneNumber, _.Fab().Faker.Phone.PhoneNumber());
            }
            
            [Test]
            public void Credit_card_is_required()
            {
                _.ShouldBeInvalid(_request, x => x.CreditCard, string.Empty);
                _.ShouldBeValid(_request, x => x.CreditCard, _.Faker().Finance.CreditCardNumber());
            }
            
            [Test]
            public void Amount_is_required_higher_than_5()
            {
                _.ShouldBeInvalid(_request, x => x.Amount, 0);
                _.ShouldBeInvalid(_request, x => x.Amount, -10);
                _.ShouldBeInvalid(_request, x => x.Amount, 4);
                _.ShouldBeValid(_request, x => x.Amount, 5);
            }
        }
    }
    // #featuretest
}