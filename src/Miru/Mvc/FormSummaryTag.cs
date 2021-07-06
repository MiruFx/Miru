using HtmlTags;

namespace Miru.Mvc
{
    public class FormSummaryTag : HtmlTag
    {
        public FormSummaryTag(string formSummaryId) : base("div")
        {
            Id(formSummaryId);
            Attr("hidden", "hidden");

            // Attr("data-controller", "form-summary");
        }
    }
}