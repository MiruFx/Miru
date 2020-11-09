using System;
using Miru;
using Miru.Domain;

namespace Mong.Payments
{
    public class AlmostRealPayPau : IPayPau
    {
        public PayPauResult Charge(string creditCard, in decimal amount)
        {
            if (creditCard.Equals("666"))
                throw new DomainException("The credit card payment was denied. Try another card or contact your card's administrator");
            
            App.Log.Information($"PayPau charge {amount} on card {creditCard}");

            return new PayPauResult
            {
                TransactionId = Guid.NewGuid().ToString()
            };
        }
    }
}