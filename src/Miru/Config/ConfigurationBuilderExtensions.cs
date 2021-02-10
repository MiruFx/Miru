using System.IO;
using Microsoft.Extensions.Configuration;
using Miru.Core;

namespace Miru.Config
{
    public static class ConfigurationBuilderExtensions
    {
        public static IConfigurationBuilder AddConfigYml(
            this IConfigurationBuilder builder,
            string environment = "",
            bool optional = true,
            bool reload = true)
        {
            var solution = App.Solution;

            if (solution.ConfigDir.Exists() == false)
                return builder;
                
            var configYml = Path.Combine(solution.ConfigDir, MiruSolution.ConfigYml(environment));
            
            return builder.AddYamlFile(configYml, optional, reload);
        }
        
        public static IConfigurationBuilder AddConfigYml(
            this IConfigurationBuilder builder,
            string configDir,
            string environment,
            bool optional = true,
            bool reload = true)
        {
            var configYml = Path.Combine(configDir, MiruSolution.ConfigYml(environment));
            
            return builder.AddYamlFile(configYml, optional, reload);
        }
    }
}