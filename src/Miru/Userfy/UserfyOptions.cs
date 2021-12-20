namespace Miru.Userfy;

public class UserfyOptions
{
    public string RequiredLoginMessage { get; set; } = "Please, login or create a new account";
    
    public object AfterLoginFeature { get; set; } 
}