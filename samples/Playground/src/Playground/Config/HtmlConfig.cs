using Miru.Html;

namespace Playground.Config
{
    public class HtmlConfig : HtmlConfiguration
    {
        public HtmlConfig()
        {
            this.AddTwitterBootstrap();

            // editors
            this.AddFileEditor();

            
            Displays.IfPropertyIs<bool>().ModifyWith(x =>
            {
                var value = x.Value<bool>();

                x.CurrentTag.Text(value ? "Yes" : "No");
            });
            
            // TODO: move to ScrollToSummary
            FormSummaries.Always.Attr("data-controller", "form-summary");
        }
    }
}
