namespace Miru.Domain
{
    public interface IUserStamped
    {
        long CreatedById { get; set; }
        
        long UpdatedById { get; set; }
    }
}