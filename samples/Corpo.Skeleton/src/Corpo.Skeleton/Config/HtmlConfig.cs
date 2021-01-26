using Miru.Html;
using Miru.RailsUjs;

namespace Corpo.Skeleton.Config
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