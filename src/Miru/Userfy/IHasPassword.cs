namespace Miru.Userfy
{
    public interface IHasPassword
    {
        string HashedPassword { get; set; }
    }
}