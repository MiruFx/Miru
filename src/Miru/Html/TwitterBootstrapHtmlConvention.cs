
namespace Miru.Html
{
    public static class TwitterBootstrapHtmlConvention
    {
        public static HtmlConfiguration AddTwitterBootstrap(this HtmlConfiguration html)
        {
            html.Editors.Always.AddClass("form-control");
            
            html.Selects.Always.AddClass("form-select");
            
            html.Submits.Always.AddClass("btn btn-primary");
            
            // cfg.FormSummaries.Always.AddClass("alert alert-danger d-none");
            
            html.Editors.IfPropertyIs<bool>().ModifyWith(m => m.CurrentTag.Class("form-check-input"));
            
            html.TableHeader.IfPropertyIs<decimal>().AddClass("text-right");
            html.TableHeader.IfPropertyIs<long>().AddClass("text-right");
            html.TableHeader.IfPropertyIs<int>().AddClass("text-right");
            
            html.Cells.IfPropertyIs<decimal>().AddClass("text-right");
            html.Cells.IfPropertyIs<long>().AddClass("text-right");
            html.Cells.IfPropertyIs<int>().AddClass("text-right");

            html.Editors.IfPropertyHasAttribute<RadioAttribute>()
                .ModifyTag(tag => tag.Class("form-check-input"));
            
            html.Editors.IfPropertyHasAttribute<CheckboxAttribute>()
                .ModifyTag(tag => tag.Class("form-check-input"));

            html.ValidationMessages.Always.AddClass("invalid-feedback");

            return html;
        }

        public static HtmlConfiguration AddMiruBootstrapLayout(this HtmlConfiguration cfg)
        {
            // only when form is horizontal?
            // cfg.Labels.Always.AddClass("col-md-4 col-form-label text-md-right");
            
            cfg.Labels.IfPropertyIs<bool>().ModifyTag(tag => tag.Class("form-check-label"));

            return cfg;
        }
    }
}