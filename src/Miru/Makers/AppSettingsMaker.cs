using System.IO;
using Miru.Core;

namespace Miru.Makers
{
    public static class AppSettingsMaker
    {
        public static void AppSettings(this Maker maker, string environment)
        {
            var destination = A.Path(maker.Solution.AppDir, $"appSettings.{environment}.yml");

            if (File.Exists(destination))
            {
                Console2.GreyLine($"\tSkip\t{maker.Solution.Relative(destination)}");
                return;
            }

            var appSettingsExample = A.Path(maker.Solution.AppDir, $"appSettings-example.yml");
            
            // appSettings-example.yml exists, copy from it
            if (File.Exists(appSettingsExample))
            {
                Console2.GreyLine($"\tCopy\t{maker.Solution.Relative(appSettingsExample)}");
                Console2.GreyLine($"\tTo\t{maker.Solution.Relative(destination)}");
                
                File.Copy(appSettingsExample, destination);
                
                return;
            }
            
            // appSettings-example.yml does not exists, create from template
            maker.Template(
                "appSettings-example.yml", 
                new { }, 
                destination);
        }
    }
}