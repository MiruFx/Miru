namespace Miru.Domain
{
    public interface IAuditable
    {
        Audit Audit { get; }
    }
}