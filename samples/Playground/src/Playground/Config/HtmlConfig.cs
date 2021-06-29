using Miru.Html;

namespace Playground.Config
{
    public class HtmlConfig : HtmlConfiguration
    {
        public HtmlConfig()
        {
            this.AddTwitterBootstrap();
            
            this.AddMiruBootstrapLayout();
            
            TableHeader.IfPropertyIs<decimal>().AddClass("text-end");
            Cells.IfPropertyIs<decimal>().AddClass("text-end");
            
            TableHeader.IfPropertyIs<bool>().AddClass("text-center");
            Cells.IfPropertyIs<bool>().AddClass("text-center");
            
            Displays.IfPropertyIs<bool>().ModifyWith(x =>
            {
                var value = x.Value<bool>();

                x.CurrentTag.Text(value ? "Yes" : "No");
            });
        }
    }
}
