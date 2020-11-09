using System.IO;
using Miru.Core;
using Scriban;

namespace Miru.Mailing
{
    public class LiquidRenderer
    {
        private readonly MiruSolution _solution;

        public LiquidRenderer(MiruSolution solution)
        {
            _solution = solution;
        }

        public string Render(string path, object model = null)
        {
            var template = GetTemplate(path);
            
            return template.Render(model, member => member.Name);
        }
        
        private Template GetTemplate(string templatePath)
        {
            var fullPath = _solution.AppDir / templatePath;
        
            return Template.Parse(File.ReadAllText(fullPath));
        }
    }
}