namespace Miru.Userfy
{
    public interface ICanBeAdmin : IUser
    {
        bool IsAdmin { get; set; }
    }
}