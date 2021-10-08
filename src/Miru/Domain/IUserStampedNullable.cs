namespace Miru.Domain
{
    public interface IUserStampedNullable
    {
        long? CreatedById { get; set; }
        
        long? UpdatedById { get; set; }
    }
}