using Corpo.Skeleton.Features.Examples;
using Miru.Html;

namespace Corpo.Skeleton.Config
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