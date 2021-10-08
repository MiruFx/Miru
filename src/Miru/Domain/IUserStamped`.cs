namespace Miru.Domain
{
    public interface IUserStamped<TIdType>
    {
        TIdType CreatedById { get; set; }
        
        TIdType UpdatedById { get; set; }
    }
}