using HtmlTags;
using HtmlTags.Conventions;
using HtmlTags.Conventions.Elements;

namespace Miru.Html
{
    public class SelectBuilder : IElementBuilder
    {
        public HtmlTag Build(ElementRequest request)
        {
            return new SelectTag()
                .Id(request.ElementId)
                .Name(request.ElementId);
        }
    }
}