using HtmlTags.Extended.Attributes;
using Microsoft.AspNetCore.Http;
using Miru.Html;
using Supportreon.Features.Donations;

namespace Supportreon.Config
{
    public class HtmlConfig : HtmlConfiguration
    {
        public HtmlConfig()
        {
            this.AddTwitterBootstrap();
            
            this.AddMiruBootstrapLayout();
            
            Editors.IfPropertyNameIs("Description").ModifyTag(tag => tag.MultilineMode());
            
            Editors.IfPropertyIs<IFormFile>().Attr("type", "file");
            
            // To avoid someone adding a real credit card
            Editors
                .IfPropertyNameIs(nameof(DonationNew.Command.CreditCard))
                .ModifyTag(tag => tag.MaxLength(10));
        }
    }
}
