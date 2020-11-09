using Miru.Domain;
using Miru.Testing;
using NUnit.Framework;
using Mong.Domain;
using Mong.Features.Topups;
using Mong.Payments;
using NSubstitute;
using Shouldly;

namespace Mong.Tests.Domain
{
    public class TopupTest : DomainTest
    {
        // #domaintest
        [Test]
        public void Create_new_topup()
        {
            // arrange
            var provider = _.Make<Provider>(m => m.Amounts = "10,20,30");
            var command = _.Make<TopupNew.Command>(m => m.Amount = 10);
            var user = _.Make<User>();
            
            // act
            var topup = new Topup(command, user, provider);
            
            // assert
            topup.Email.ShouldBe(user.Email);
            topup.Name.ShouldBe(user.Name);
            
            topup.Provider.ShouldBe(provider);
            topup.Amount.ShouldBe(command.Amount);
            topup.PhoneNumber.ShouldBe(command.PhoneNumber);
            
            topup.Status.ShouldBe(TopupStatus.Created);
        }
        // #domaintest
        
        [Test]
        public void Pay_topup()
        {
            // arrange
            var provider = _.Make<Provider>(m => m.Amounts = "10,20,30");
            var command = _.Make<TopupNew.Command>(m => m.Amount = 10);
            var user = _.Make<User>();
            var paymentResult = _.Make<PayPauResult>();
            var topup = new Topup(command, user, provider);
            
            _.Get<IPayPau>().Charge(command.CreditCard, command.Amount).Returns(paymentResult);
            
            // act
            topup.Pay(_.Get<IPayPau>(), command.CreditCard);
            
            // assert
            topup.PaymentId.ShouldBe(paymentResult.TransactionId);
            topup.Status.ShouldBe(TopupStatus.Paid);
        }
        
        [Test]
        public void Topup_complete()
        {
            // arrange
            var topup = new Topup();
            
            // act
            topup.Complete();
            
            // assert
            topup.Status.ShouldBe(TopupStatus.Completed);
        }
        
        [Test]
        public void Cant_create_topup_if_amount_is_not_supported_by_the_provider()
        {
            // arrange
            var provider = _.Make<Provider>(m => m.Amounts = "10,20,30");
            var command = _.Make<TopupNew.Command>(m => m.Amount = 100);
            var user = _.Make<User>();
            
            // act
            Should.Throw<DomainException>(() => new Topup(command, user, provider));
        }
    }
}