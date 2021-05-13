using Miru.Html;

namespace Pantanal.Config
{
    public class HtmlConfig : HtmlConfiguration
    {
        public HtmlConfig()
        {
            this.AddTwitterBootstrap();
            
            this.AddMiruBootstrapLayout();
        }
    }
}
