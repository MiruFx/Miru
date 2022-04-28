namespace Miru.Userfy;

public class UserfyOptions
{
    public string RequiredLoginMessage { get; set; } = "Please, login or create a new account";

    public string UserPasswordNotFound { get; set; } = "User and password not found";
    
    public object AfterLoginFeature { get; set; } 
}