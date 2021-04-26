using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Miru.Html.Tags
{
    [HtmlTargetElement("miru-summary")]
    public class SummaryTagHelper : NoPropertyTagHelper
    {
        protected override string Category { get; } = nameof(HtmlConfiguration.FormSummaries);
    }
}