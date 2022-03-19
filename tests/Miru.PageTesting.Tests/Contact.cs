namespace Miru.PageTesting.Tests;

public class Contact
{
    public string Name { get; set; }
    
    public List<Address> Addresses { get; } = new List<Address>();
}