using Miru.Html;

namespace Miru.Tests.Html
{
    public class TestingAntiForgeryAccessor : IAntiforgeryAccessor
    {
        public string RequestToken { get; }
    
        public string FormFieldName { get; }
    
        public bool HasToken { get; }
    
        public TestingAntiForgeryAccessor()
        {
            RequestToken = nameof(RequestToken);
            FormFieldName = nameof(FormFieldName);
            HasToken = true;
        }
    }
}