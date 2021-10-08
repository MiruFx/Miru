using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.CommandLine;
using Microsoft.Extensions.Configuration.EnvironmentVariables;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Configuration.Memory;
using Microsoft.Extensions.Hosting;
using Miru.Consolables;
using Miru.Core;
using NetEscapades.Configuration.Yaml;

namespace Miru.Config
{
    /*
    https://gist.github.com/amoerie/22a1fbfd0196b85b2cc15af0349af0f4
    */
    public class ConfigShowConsolable : Consolable
    {
        public ConfigShowConsolable() 
            : base("config.show", "Show the configuration will be used")
        {
            AddOption(new Option<string>("--environment").WithAlias("--e"));
        }

        public class ConsolableHandler : IConsolableHandler
        {
            private readonly IHostEnvironment _hostEnvironment;
            private readonly IConfiguration _configuration;
            
            public ConsolableHandler(
                IHostEnvironment hostEnvironment,
                IConfiguration configuration)
            {
                _hostEnvironment = hostEnvironment;
                _configuration = configuration;
            }

            public Task Execute()
            {
                Console2.Yellow("App version: ");
                Console2.Line(
                    $"{Assembly.GetEntryAssembly().GetName().Name} {Assembly.GetEntryAssembly().GetName().Version}");

                Console2.Yellow("Miru version: ");
                Console2.Line(typeof(App).Assembly.GetName().Version?.ToString());
                Console.WriteLine();

                Console2.YellowLine("Host:");
                Console2.Line(_hostEnvironment.ToYml());

                Console2.YellowLine("All Configurations: ");
                Console.WriteLine(GetDebugView(_configuration as IConfigurationRoot));

                return Task.CompletedTask;
            }

            private static string GetDebugView(IConfigurationRoot configurationRoot)
            {
                var builder = new StringBuilder();

                var providers = GetProviders(configurationRoot).ToList();

                builder.AppendLine(
                    "Configuration providers in order of preference: (values from providers that come first override values from providers that come later)");

                for (var index = 0; index < providers.Count; index++)
                {
                    var provider = providers[index];
                    builder.Append("Provider ");
                    builder.Append(index);
                    builder.Append(" = ");
                    builder.Append(ProviderToString(provider));
                    builder.AppendLine();
                }

                builder.AppendLine();

                builder.AppendLine("Configuration values: ");

                BuildDebugView(providers, builder, configurationRoot.GetChildren(), "");

                return builder.ToString();
            }

            private static void BuildDebugView(
                List<IConfigurationProvider> providers,
                StringBuilder stringBuilder,
                IEnumerable<IConfigurationSection> children,
                string indent)
            {
                foreach (var child in children)
                {
                    (var value, IConfigurationProvider provider) = GetValueAndProvider(providers, child.Path);

                    if (provider != null)
                    {
                        stringBuilder
                            .Append(indent)
                            .Append(child.Key)
                            .Append('=')
                            .Append(value)
                            .Append(" (")
                            .Append(ProviderToString(provider))
                            .AppendLine(")");
                    }

                    BuildDebugView(providers, stringBuilder, child.GetChildren(), indent + child.Key + ".");
                }
            }

            private static IEnumerable<IConfigurationProvider> GetProviders(IConfigurationRoot configurationRoot)
            {
                foreach (var provider in configurationRoot.Providers.Reverse())
                {
                    switch (provider)
                    {
                        case EnvironmentVariablesConfigurationProvider environmentVariablesConfigurationProvider:
                            bool yieldProvider;
                            try
                            {
                                var prefix = typeof(EnvironmentVariablesConfigurationProvider)
                                        .GetField("_prefix", BindingFlags.Instance | BindingFlags.NonPublic)!
                                        .GetValue(environmentVariablesConfigurationProvider)
                                    as string;

                                yieldProvider = !string.IsNullOrWhiteSpace(prefix);
                            }
                            catch
                            {
                                yieldProvider = false;
                            }

                            if (yieldProvider)
                                yield return environmentVariablesConfigurationProvider;

                            break;
                        case ChainedConfigurationProvider chainedConfigurationProvider:
                            var innerProviders = new List<IConfigurationProvider>();
                            try
                            {
                                var configField = typeof(ChainedConfigurationProvider).GetField("_config",
                                    BindingFlags.Instance | BindingFlags.NonPublic);
                                var innerConfigurationRoot =
                                    configField!.GetValue(chainedConfigurationProvider) as IConfigurationRoot;

                                foreach (var innerProvider in GetProviders(innerConfigurationRoot!))
                                {
                                    innerProviders.Add(innerProvider);
                                }
                            }
                            catch
                            {
                                innerProviders.Add(chainedConfigurationProvider);
                            }

                            foreach (var innerProvider in innerProviders)
                                yield return innerProvider;

                            break;
                        default:
                            yield return provider;
                            break;
                    }
                }
            }

            private static string ProviderToString(IConfigurationProvider provider)
            {
                switch (provider)
                {
                    case JsonConfigurationProvider json:
                        return $"JSON file {json.Source.Path} (reload on change = {json.Source.ReloadOnChange})";
                    case YamlConfigurationProvider yml:
                        return $"YAML file {yml.Source.Path} (reload on change = {yml.Source.ReloadOnChange})";
                    case EnvironmentVariablesConfigurationProvider env:
                        try
                        {
                            var prefix = typeof(EnvironmentVariablesConfigurationProvider)
                                .GetField("_prefix", BindingFlags.Instance | BindingFlags.NonPublic)!
                                .GetValue(env);
                            return $"System environment variables that start with '{prefix}'";
                        }
                        catch
                        {
                            return $"System environment variables";
                        }
                    case CommandLineConfigurationProvider:
                        return $"Command line arguments";
                    case MemoryConfigurationProvider:
                        return $"In-memory configuration";
                    default:
                        return $"Custom provider: {provider.GetType()}";
                }
            }

            private static (string Value, IConfigurationProvider Provider) GetValueAndProvider(
                IEnumerable<IConfigurationProvider> providers,
                string key)
            {
                foreach (var provider in providers)
                {
                    if (provider.TryGet(key, out string value))
                    {
                        return (value, provider);
                    }
                }

                return (null, null);
            }
        }
    }
}