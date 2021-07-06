using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Miru.Html.Tags
{
    [HtmlTargetElement("miru-th")]
    public class ThTagHelper : MiruHtmlTagHelper
    {
        protected override string Category => nameof(HtmlConfiguration.TableHeader);

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var th = GetHtmlTag(nameof(HtmlConfiguration.TableHeader));
            
            var childContent = await output.GetChildContentAsync();

            if (For == null)
            {
                SetOutput(th, output);
                await Task.CompletedTask;
                return;
            }
            
            if (childContent.IsEmptyOrWhiteSpace)
            {
                var span = HtmlGenerator.TagFor(For, nameof(HtmlConfiguration.DisplayLabels));
                    // .Id(GetId());
                
                th.Children.Add(span);
            }
            else
            {
                th.AppendHtml(childContent.GetContent());
                // th.Id(GetId());
                
                childContent.Clear();
            }

            SetOutput(th, output);
        }
    }
}