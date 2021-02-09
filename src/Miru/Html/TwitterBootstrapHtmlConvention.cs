
namespace Miru.Html
{
    public static class TwitterBootstrapHtmlConvention
    {
        public static HtmlConvention AddTwitterBootstrap(this HtmlConvention cfg)
        {
            cfg.Editors.Always.AddClass("form-control");
            
            cfg.Selects.Always.AddClass("form-select");
            
            cfg.Submits.Always.AddClass("btn btn-primary");
            
            // cfg.FormSummaries.Always.AddClass("alert alert-danger d-none");
            
            cfg.Editors.IfPropertyIs<bool>().ModifyWith(m => m.CurrentTag.Class("form-check-input"));
            
            cfg.TableHeader.IfPropertyIs<decimal>().AddClass("text-right");
            cfg.TableHeader.IfPropertyIs<long>().AddClass("text-right");
            cfg.TableHeader.IfPropertyIs<int>().AddClass("text-right");
            
            cfg.Cells.IfPropertyIs<decimal>().AddClass("text-right");
            cfg.Cells.IfPropertyIs<long>().AddClass("text-right");
            cfg.Cells.IfPropertyIs<int>().AddClass("text-right");

            return cfg;
        }

        public static HtmlConvention AddMiruBootstrapLayout(this HtmlConvention cfg)
        {
            // only when form is horizontal?
            // cfg.Labels.Always.AddClass("col-md-4 col-form-label text-md-right");
            
            cfg.Labels.IfPropertyIs<bool>().ModifyTag(tag => tag.Class("form-check-label"));

            return cfg;
        }
    }
}