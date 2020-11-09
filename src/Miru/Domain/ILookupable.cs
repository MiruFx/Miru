namespace Miru.Domain
{
    public interface ILookupable
    {
        string Value { get; }
        
        string Display { get; }
    }
}