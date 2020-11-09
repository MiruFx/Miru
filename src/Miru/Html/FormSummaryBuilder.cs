using HtmlTags;
using HtmlTags.Conventions;
using HtmlTags.Conventions.Elements;

namespace Miru.Html
{
    public class FormSummaryBuilder : IElementBuilder
    {
        public HtmlTag Build(ElementRequest request)
        {
            return new HtmlTag("div").AddClass("form-summary");
        }
    }
}