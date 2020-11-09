using System;
using Miru;

namespace Mong.Payments
{
    public class MobileProvider : IMobileProvider
    {
        public void Topup(string phoneNumber, decimal amount)
        {
            if (phoneNumber.Equals("666"))
                throw new Exception($"Topup of phone number {phoneNumber} was denied. Phone number does not exist");
            
            App.Log.Information($"MobileProvider accept topup of {amount} for the phone {phoneNumber}");
        }
    }
}