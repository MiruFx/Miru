namespace Mong.Payments
{
    public interface IPayPau
    {
        PayPauResult Charge(string creditCard, in decimal amount);
    }
}