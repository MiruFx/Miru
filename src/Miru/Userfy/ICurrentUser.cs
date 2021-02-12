namespace Miru.Userfy
{
    public interface ICurrentUser
    {
        long Id { get; }
        
        string Display { get; }
        
        bool IsLogged { get; }
        
        bool IsAnonymous { get; }
    }
}