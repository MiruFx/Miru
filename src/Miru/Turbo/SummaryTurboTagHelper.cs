using Microsoft.AspNetCore.Razor.TagHelpers;
using Miru.Html;
using Miru.Html.Tags;

namespace Miru.Turbo
{
    [HtmlTargetElement("miru-summary-turbo")]
    public class SummaryTurboTagHelper : NoPropertyTagHelper
    {
        protected override string Category { get; } = nameof(HtmlConvention.FormSummaries);
    }
}