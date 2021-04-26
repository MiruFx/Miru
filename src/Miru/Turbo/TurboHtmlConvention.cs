
using Miru.Html;

namespace Miru.Turbo
{
    public static class TurboHtmlConvention
    {
        public static HtmlConfiguration AddTurbo(this HtmlConfiguration cfg)
        {
            // cfg.Forms.Always.Attr("data-turbo", "false");
            
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
    }
}