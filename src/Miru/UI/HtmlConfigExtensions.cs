using Miru.Html;

namespace Miru.UI
{
    public static class HtmlConfigExtensions
    {
        public static HtmlConfiguration AddTwitterBootstrap(this HtmlConfiguration html)
        {
            // form summaries
            html.FormSummaries.Always.AddClass("alert alert-danger");
            
            // tables
            html.Tables.Always.AddClass("table table-striped table-hover");
            
            html.TableHeaders.IfPropertyIsNumber().AddClass("text-end");
            html.TableHeaders.IfPropertyIs<bool>().AddClass("text-center");
            
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

        public static HtmlConfiguration AddMiruForm(this HtmlConfiguration html)
        {
            html.Forms.Always.ModifyTag(tag =>
            {
                tag.Attr("data-controller", "form");
            });

            return html;
        }
        
        public static HtmlConfiguration AddMiruFormSummary(this HtmlConfiguration html)
        {
            html.FormSummaries.Always.ModifyTag(tag =>
            {
                tag.Attr("hidden", "hidden");
                tag.Attr("data-controller", "form-summary");
            });

            return html;
        }
        
        public static HtmlConfiguration AddRequiredLabels(this HtmlConfiguration html)
        {
            html.Labels
                .If(x => x.FindModel().GetType().Implements<INoRequiredLabels>() == false
                         && x.FindModel().GetType().IsRequestCommand()
                         && new RequiredLabelModifier().Matches(x))
                .ModifyWith<RequiredLabelModifier>();  

            return html;
        }
    }
}