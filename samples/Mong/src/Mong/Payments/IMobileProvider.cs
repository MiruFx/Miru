namespace Mong.Payments
{
    public interface IMobileProvider
    {
        void Topup(string phoneNumber, decimal amount);
    }
}