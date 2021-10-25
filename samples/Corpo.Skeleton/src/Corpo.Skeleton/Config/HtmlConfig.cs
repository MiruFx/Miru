using Miru.Html;
using Miru.UI;

namespace Corpo.Skeleton.Config;

public class HtmlConfig : HtmlConfiguration
{
    public HtmlConfig()
    {
        this.AddTwitterBootstrap();
        this.AddMiruForm();
        this.AddMiruFormSummary();
        this.AddRequiredLabels();
    }
}