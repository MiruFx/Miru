namespace Miru.Domain
{
    public interface ISoftDeletable
    {
        bool IsDeleted { get; set; }
    }
}