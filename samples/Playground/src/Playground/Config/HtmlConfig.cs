using System.Globalization;
using Miru.Html;
using Miru.Html.HtmlConfigs;
using Miru.Html.HtmlConfigs.Core;
using Miru.Html.Tags;

namespace Playground.Config;

public class HtmlConfig : HtmlConventions
{
    public HtmlConfig()
    {
        // basic miru
        this.AddDefaultHtml();

        // miru's 3rd party integrations
        this.AddMiruHotwired();
        this.AddBootstrapStyles();
    }
}

public static class BootstrapHtmlConventionsExtensions
{
    public static HtmlConventions AddBootstrapStyles(this HtmlConventions html)
    {
        // form summaries
        html.FormSummaries.Always.AddClass("alert alert-danger");
            
        // tables
        html.Tables.Always.AddClass("table");
            
        html.TableHeaders.IfNumber().AddClass("text-end");
        html.TableHeaders.If<bool>().AddClass("text-center");
            
        html.TableCells.If<bool>().AddClass("text-center");
        html.TableCells.IfNumber().AddClass("text-end text-nowrap");

        // editors
        html.Inputs.Always.AddClass("form-control");
        html.Inputs.If<bool>().SetClass("form-check-input");
        html.Inputs.IfTag(tag => tag.Attributes.IsTypeRadio()).SetClass("form-check-input");
        html.Inputs.IfTag(tag => tag.Attributes.IsTypeCheckbox()).SetClass("form-check-input");
        html.Inputs.IfHas<CheckboxAttribute>().AddClass("form-check-input");
        html.Selects.Always.AddClass("form-select");
        html.Submits.Always.AddClass("btn btn-primary");

        // validations
        html.ValidationMessages.Always.AddClass("invalid-feedback");
            
        // labels
        html.Labels.If<bool>().AddClass("form-check-label");

        return html;
    }
}