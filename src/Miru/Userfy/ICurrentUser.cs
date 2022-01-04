namespace Miru.Userfy
{
    public interface ICurrentUser
    {
        long Id { get; }
        
        string Display { get; }
        
        bool IsAuthenticated { get; }
        
        bool IsAnonymous { get; }
    }
}