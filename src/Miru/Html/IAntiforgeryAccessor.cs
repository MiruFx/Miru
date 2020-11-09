namespace Miru.Html
{
    public interface IAntiforgeryAccessor
    {
        string RequestToken { get; }
        
        string FormFieldName { get; }

        bool HasToken { get; }
    }
}