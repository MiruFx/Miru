using Miru.Core;

namespace Miru.Makers
{
    public static class MailMaker
    {
        public static void Mail(this Maker maker, string @in, string name)
        {
            var input = new
            {
                Name = name, 
                In = maker.Namespace(@in),
            };
            
            maker.Template("Mailable", input, maker.Solution.FeaturesDir / maker.Expand(@in) / $"{name}Mail.cs");
            
            maker.Template("MailTemplate", input, maker.Solution.FeaturesDir / maker.Expand(@in) / $"{name}Mail.cshtml");   
        }
    }
}