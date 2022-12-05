namespace Miru.Html.HtmlConfigs;

public static class EmailHtmlConventionsExtensions
{
    public static HtmlConventions AddEmailModifiers(this HtmlConventions html)
    {
        html.Inputs.If(req => req.PropertyName.EndsWith("Email")).Modify((tag, req) =>
        {
            tag.Attributes.SetAttribute("autocapitalize", "off");
        });
        
        return html;
    }
}