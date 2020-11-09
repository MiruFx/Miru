using Miru.Core;

namespace Miru.Makers
{
    public static class EmailMaker
    {
        public static void Email(this Maker maker, string @in, string name, string action)
        {
            var input = new
            {
                Name = name, 
                In = maker.Namespace(@in),
                Action = action
            };
            
            maker.Template("Mailable", input, maker.Solution.FeaturesDir / maker.Expand(@in) / $"{name}{action}Mail.cs");
            
            maker.Template("MailTemplate", input, maker.Solution.FeaturesDir / maker.Expand(@in) / $"{name}{action}Mail.cshtml");
        }
    }
}