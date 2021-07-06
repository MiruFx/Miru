using HtmlTags;
using HtmlTags.Conventions;
using HtmlTags.Conventions.Elements;
using Miru.Mvc;

namespace Miru.Html
{
    public class FormSummaryBuilder : IElementBuilder
    {
        public HtmlTag Build(ElementRequest request)
        {
            return new FormSummaryTag(request.ElementId);
        }
    }
}