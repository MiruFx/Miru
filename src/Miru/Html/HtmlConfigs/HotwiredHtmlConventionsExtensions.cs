using Miru.Html.HtmlConfigs.Core;

namespace Miru.Html.HtmlConfigs;

public static class HotwiredHtmlConventionsExtensions
{
    public static HtmlConventions AddMiruHotwired(this HtmlConventions html)
    {
        html.Forms.Always
            .AppendAttr(HtmlAttr.DataController, HtmlAttr.Form)
            .Modify((tag, req) => 
                tag.Attributes.SetAttribute(HtmlAttr.DataFormSummary, req.Naming.FormSummaryId(req.Value)));

        html.FormSummaries.Always
            .SetAttr(HtmlAttr.Hidden, HtmlAttr.Hidden)
            .AppendAttr(HtmlAttr.DataController, HtmlAttr.FormSummary);

        return html;
    }
}