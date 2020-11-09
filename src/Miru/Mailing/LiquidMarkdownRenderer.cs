using System.Threading.Tasks;
using FluentEmail.Core.Interfaces;
using Markdig;

namespace Miru.Mailing
{
    public class LiquidMarkdownRenderer : ITemplateRenderer
    {
        private readonly LiquidRenderer _liquidRenderer;

        public LiquidMarkdownRenderer(LiquidRenderer liquidRenderer)
        {
            _liquidRenderer = liquidRenderer;
        }

        public string Parse<T>(string template, T model, bool isHtml = true)
        {
            var liquidRendered = _liquidRenderer.Render(template, model);
            
            return Markdown.ToHtml(liquidRendered);
        }

        public Task<string> ParseAsync<T>(string template, T model, bool isHtml = true)
        {
            return Task.FromResult(Parse(template, model));
        }
    }
}