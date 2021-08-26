namespace Miru.Domain
{
    public interface ILookupable
    {
        string Value { get; }
        
        string Display { get; }
    }

    public interface ILookupableEntity
    {
        long Id { get; }
        
        string Name { get; }
    }
}