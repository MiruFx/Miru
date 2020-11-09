using System;
using Miru.Domain;
using Mong.Features.Topups;
using Mong.Payments;

namespace Mong.Domain
{
    public class Topup : Entity, IBelongsToUser, ITimeStamped
    {
        public decimal Amount { get; set; }
        public long ProviderId { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public long UserId { get; set; }
        public TopupStatus Status { get; set; } 
        public string PaymentId { get; set; }
        public DateTime PaidAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public User User { get; set; }
        public Provider Provider { get; set; }

        public Topup()
        {
        }

        public Topup(TopupNew.Command request, User user, Provider provider)
        {
            if (provider.SupportsAmount(request.Amount) == false)
                throw new DomainException($"The provider {provider.Name} does not support the amount of {request.Amount}");

            User = user;
            UserId = request.UserId;

            Provider = provider;
            ProviderId = request.ProviderId;
            
            PhoneNumber = request.PhoneNumber;
            Amount = request.Amount;

            Email = user.Email;
            Name = user.Name;
        }

        public void Pay(IPayPau payPau, string creditCard)
        {
            var paymentId = payPau.Charge(creditCard, Amount).TransactionId;
            
            PaymentId = paymentId;
            Status = TopupStatus.Paid;
            PaidAt = DateTime.Now;
        }
        
        public void Complete()
        {
            Status = TopupStatus.Completed;
        }

        public void Error()
        {
            Status = TopupStatus.Error;
        }
    }
}