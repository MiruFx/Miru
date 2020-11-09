namespace Miru.Userfy
{
    public interface ILoginRequest
    {
        string Email { get; set; }
        
        string Password { get; set; }
    }
}