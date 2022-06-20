using HtmlTags;
using HtmlTags.Conventions;
using HtmlTags.Conventions.Elements;

namespace Miru.Html;

public class TableBuilder : IElementBuilder
{
    public HtmlTag Build(ElementRequest request)
    {
        return new HtmlTag("table");
    }
}