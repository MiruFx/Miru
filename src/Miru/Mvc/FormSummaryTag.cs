using HtmlTags;

namespace Miru.Mvc
{
    public class FormSummaryTag : HtmlTag
    {
        public FormSummaryTag(string formSummaryId) : base("div")
        {
            // TODO: parse css selector to HtmlTag (at least basic .class #id attribute
            Id(formSummaryId);
            AddClass("form-summary");
            AddClass("alert");
            AddClass("alert-danger");
            Attr("data-controller", "form-summary");
        }
    }
}