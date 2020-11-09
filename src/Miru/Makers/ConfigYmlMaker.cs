using Miru.Core;

namespace Miru.Makers
{
    public static class ConfigYmlMaker
    {
        public static void ConfigYml(this Maker maker, string environment)
        {
            maker.Template("Config", new { }, A.Path(maker.Solution.ConfigDir, maker.Solution.GetConfigYml(environment)));
        }
    }
}