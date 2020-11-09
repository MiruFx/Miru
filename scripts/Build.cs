using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CommandLine;
using Miru;
using static Bullseye.Targets;
using static SimpleExec.Command;

namespace Scripts
{
    internal class Build
    {
        public class Options
        {
            [Value(0)]
            public string Target { get; set; }
            
            [Option('k', "key", Required = false, HelpText = "Nuget server's API key to push packages")]
            public string Key { get; set; }
            
            [Option('d', "debug", Required = false, HelpText = "Compile in 'Debug' mode instead of 'Release'")]
            public bool Debug { get; set; }
        }
        
        // package id and if include symbols
        private static readonly Dictionary<string, bool> Packages = new Dictionary<string, bool>
        {
            { "Miru.Core", true },
            { "Miru", true },
            { "Miru.Fabrication", true },
            { "Miru.Testing", true },
            { "Miru.PageTesting", true }, 
            { "Miru.PageTesting.Chrome", true }, 
            { "Miru.PageTesting.Firefox", true }, 
            { "MiruCli", false }
        };

        private static int Main(string[] args)
        {
            var success = true;
            
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(o =>
                {
                    RunBuild(o, args);
                })
                .WithNotParsed(e =>
                {
                    success = false;
                });

            return success ? 0 : -1;
        }

        private static void HandleErrors(IEnumerable<Error> obj)
        {
            throw new NotImplementedException();
        }

        public static void RunBuild(Options option, string[] args)
        {
            var buildConfig = option.Debug ? "Debug" : "Release";

            Target("default", DependsOn("compile"));

            Target("clean", () =>
                EnsureDirectoriesDeleted("artifacts", "packages"));

            Target("restore", DependsOn("clean"), () =>
                Run("dotnet", "restore"));

            Target("compile", DependsOn("restore"), () =>
                Run("dotnet", $"build -c {buildConfig} --no-restore"));
            
             Target("test", DependsOn("compile"), () =>
                Run("dotnet", $"test -c {buildConfig}", workingDirectory: @"tests/Miru.Tests"));
            
            Target("pack", DependsOn("compile"), () =>
            {
                foreach (var releasable in Packages)
                {
                    Run("dotnet", $"pack src\\{releasable.Key} -c {buildConfig} --no-build --nologo");
                }
            });
            
            Target("publish-dev", DependsOn("export-stubs", "compile", "test", "mong-test", "pack"), () =>
            {
                PushPackages(option.Key, "https://f.feedz.io/miru/miru/nuget");
            });
            
            Target("publish-nuget", DependsOn("export-stubs", "pack"), () =>
            {
                PushPackages(option.Key, "https://api.nuget.org/v3/index.json");
            });
            
            Target("publish-docs", () =>
            {
                Shell("npm", "run docs:build");
                Run("git", "init", workingDirectory: @"docs/.vuepress/dist");
                Run("git", "add -A", workingDirectory: @"docs/.vuepress/dist");
                Run("git", "commit -m 'Deploy'", workingDirectory: @"docs/.vuepress/dist");
                Run("git", "remote add origin https://github.com/MiruFx/mirufx.github.io.git", workingDirectory: @"docs/.vuepress/dist");
                Run("git", "push --set-upstream origin master -f", workingDirectory: @"docs/.vuepress/dist");
            });

            Target("mong-test", () =>
            {
                Run("dotnet", "test", workingDirectory: @"samples\Mong\tests\Mong.Tests");
            });
            
            Target("mong-test-all", () =>
            {
                Run("dotnet", "test", workingDirectory: @"samples\Mong");
            });
            
            Target("export-stubs", () =>
            {
                ExportStubs.Export();
            });
            
            Target("test-new-solution", () =>
            {
                TestNewSolution.Test();
            });
            
            Target("deep-clean", () => DeepClean());
            
            RunTargetsAndExit(new[] { option.Target });
        }

        private static void DeepClean()
        {
            var searchIn = new[] { "src", "samples", "tests" };
            var searchFor = new [] { "node_modules", "bin", "obj" };
            
            var dirs = searchIn.SelectMany(searchDir => 
                searchFor.SelectMany(dir => Directory.GetDirectories(searchDir, dir,SearchOption.AllDirectories)));

            foreach (var dir in dirs)
            {
                if (Directory.Exists(dir))
                {
                    Console.WriteLine($"Deleting {dir}");
                    Directory.Delete(dir);
                }
            }
        }

        private static void EnsureDirectoriesDeleted(params string[] paths)
        {
            foreach (var path in paths)
            {
                if (Directory.Exists(path))
                {
                    var dir = new DirectoryInfo(path);
                    DeleteDirectory(dir);
                }
            }
        }

        private static void DeleteDirectory(DirectoryInfo baseDir)
        {
            baseDir.Attributes = FileAttributes.Normal;
            foreach (var childDir in baseDir.GetDirectories())
                DeleteDirectory(childDir);

            foreach (var file in baseDir.GetFiles())
                file.IsReadOnly = false;

            baseDir.Delete(true);
        }

        private static void Shell(string command, string args) =>
            Run(command, args, windowsName: "cmd.exe", windowsArgs: $"/c {command} {args}");

        private static void PushPackages(string serverApiKey, string serverUrl)
        {
            if (string.IsNullOrEmpty(serverApiKey))
            {
                Console.WriteLine("No Nuget API key found. Skip publishing");
                return;
            }
            
            var param = $" -s {serverUrl} -k {serverApiKey} --skip-duplicate";
                
            var paramNoSymbols = $" -s {serverUrl} -k {serverApiKey} --skip-duplicate";
                
            foreach (var (package, withSymbols) in Packages)
            {
                if (withSymbols)
                {
                    Run("dotnet", $"nuget push {Path.Combine("packages", package)}.*.nupkg {param}", noEcho: true);
                    Run("dotnet", $"nuget push {Path.Combine("packages", package)}.*.snupkg {paramNoSymbols}", noEcho: true);
                }
                else
                    Run("dotnet", $"nuget push {Path.Combine("packages", package)}.*.nupkg {paramNoSymbols}", noEcho: true);
            }
        }
    }
}
