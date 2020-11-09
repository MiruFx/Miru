using Miru.Html;

namespace Miru.RailsUjs
{
    public static class HtmlConventionExtensions
    {
        public static HtmlConvention AddRailsUjs(this HtmlConvention cfg)
        {
            cfg.Forms.Always.Attr("data-remote", "true");
            
            return cfg;
        }
    }
}