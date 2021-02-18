using HtmlTags.Extended.Attributes;
using Miru.Html;
using Supportreon.Features.Donations;

namespace Supportreon.Config
{
    public class HtmlConfig : HtmlConvention
    {
        public HtmlConfig()
        {
            this.AddTwitterBootstrap();
            
            this.AddMiruBootstrapLayout();
            
            Editors.IfPropertyNameIs("Description").ModifyTag(tag => tag.MultilineMode());
            
            // To avoid someone adding a real credit card
            Editors
                .IfPropertyNameIs(nameof(DonationNew.Command.CreditCard))
                .ModifyTag(tag => tag.MaxLength(10));
        }
    }
}
