namespace MiruNext.Domain;

public class OrderStatuses : SmartEnum<OrderStatuses>
{
    public static OrderStatuses InProgress = new("In Progress", 1);
    public static OrderStatuses Paid = new("Paid", 2);
    public static OrderStatuses PaymentFailed = new("Payment Failed", 3);
    
    public OrderStatuses(string name, int value) : base(name, value)
    {
    }
}