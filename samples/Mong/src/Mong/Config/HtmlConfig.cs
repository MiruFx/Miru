using Miru.Html;
using Miru.RailsUjs;

namespace Mong.Config
{
    public class HtmlConfig : HtmlConvention
    {
        public HtmlConfig()
        {
            this.AddTwitterBootstrap();
            
            this.AddMiruBootstrapLayout();
            
            this.AddRailsUjs();
        }
    }
}