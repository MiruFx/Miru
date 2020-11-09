using Miru.Html;
using Miru.RailsUjs;

namespace SelfImprov.Config
{
    public class HtmlConfig : HtmlConvention
    {
        public HtmlConfig()
        {
            this.AddTwitterBootstrap();
            
            this.AddMiruBootstrapLayout();
            
            this.AddRailsUjs();
            
            Displays
                .If(m => m.Accessor.PropertyType == typeof(decimal) && m.Accessor.Name.ToLower().Contains("percent"))
                .ModifyWith(m => m.CurrentTag.Text(m.Value<decimal>().ToString("#") + "%"));

            Editors.IfPropertyIs<bool>().ModifyWith(m =>
            {
                var value = m.Value<bool>();
                if (value)
                    m.CurrentTag.Attr("checked", "checked");
            });
        }
    }
}
