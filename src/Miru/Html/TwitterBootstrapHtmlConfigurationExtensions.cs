
namespace Miru.Html
{
    public static class TwitterBootstrapHtmlConfigurationExtensions
    {
        public static HtmlConfiguration AddTwitterBootstrap(this HtmlConfiguration html)
        {
            // form summaries
            html.FormSummaries.Always.AddClass("alert alert-danger");
            
            // tables
            html.TableHeader.IfPropertyIsNumber().AddClass("text-end");
            html.TableHeader.IfPropertyIs<bool>().AddClass("text-center");
            
            html.Cells.IfPropertyIs<bool>().AddClass("text-center");
            html.Cells.IfPropertyIsNumber().AddClass("text-end");

            // editors
            html.Editors.Always.AddClass("form-control");

            html.Selects.Always.AddClass("form-select");

            html.Submits.Always.AddClass("btn btn-primary");

            html.Editors.IfPropertyIs<bool>()
                .ModifyWith(m => m.CurrentTag.Class("form-check-input"));

            html.Editors.IfPropertyHasAttribute<RadioAttribute>()
                .ModifyTag(tag => tag.Class("form-check-input"));
            
            html.Editors.IfPropertyHasAttribute<CheckboxAttribute>()
                .ModifyTag(tag => tag.Class("form-check-input"));

            // validations
            html.ValidationMessages.Always.AddClass("invalid-feedback");
            
            // labels
            html.Labels.IfPropertyIs<bool>().ModifyTag(tag => tag.Class("form-check-label"));

            return html;
        }
    }
}