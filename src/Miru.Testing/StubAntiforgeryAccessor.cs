using Miru.Html;

namespace Miru.Testing
{
    public class StubAntiforgeryAccessor : IAntiforgeryAccessor
    {
        public string RequestToken { get; } = "Token";
        public string FormFieldName { get; } = "FieldName";
        public bool HasToken => true;
    }
}