using HtmlTags;
using HtmlTags.Conventions;
using HtmlTags.Conventions.Elements;
using Miru.Core;

namespace Miru.Mvc
{
    public class DefaultDisplayLabelBuilder : IElementBuilder
    {
        public bool Matches(ElementRequest subject) => true;

        public HtmlTag Build(ElementRequest request)
        {
            return new HtmlTag(string.Empty)
                .NoTag()
                .Text(request.Accessor.Name.BreakCamelCase());
        }
    }
}
