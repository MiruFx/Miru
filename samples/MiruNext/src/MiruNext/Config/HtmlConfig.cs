using Miru.Html.HtmlConfigs;

namespace MiruNext.Config;

public class HtmlConfig : HtmlConventions
{
    public HtmlConfig()
    {
        this.AddDefaultHtml();
        this.AddMiruHotwired();
        // this.AddBootstrapStyles();
    }
}