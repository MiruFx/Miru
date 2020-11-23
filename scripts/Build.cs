using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            
            new Parser(with => with.EnableDashDash = true).ParseArguments<Options>(args)
                .WithParsed(option => RunBuild(option, args))
                .WithNotParsed(e =>
                {
                    success = false;
                });

            return success ? 0 : -1;
        }

        public static void RunBuild(Options option, string[] args)
        {
            var buildConfig = option.Debug ? "Debug" : "Release";

            Target("default", DependsOn("compile"));

            Target("clean", () =>
                EnsureDirectoriesDeleted("artifacts", "packages"));

            Target("restore", DependsOn("clean"), () =>
                Run("dotnet", "restore"));

            Target("compile", DependsOn("restore", "export-stubs"), () =>
                Run("dotnet", $"build -c {buildConfig} --no-restore"));
            
            Target("test", DependsOn("compile"), () =>
                Run("dotnet", $"test -c {buildConfig} --no-build", workingDirectory: @"tests/Miru.Tests"));
            
            Target("pack", DependsOn("compile"), () =>
            {
                foreach (var releasable in Packages)
                {
                    Run("dotnet", $"pack src\\{releasable.Key} -c {buildConfig} --no-build --nologo");
                }
            });
            
            Target("test-ci", DependsOn("test", "mong-test-all"));

            Target("publish-dev", DependsOn("test-ci", "pack"), () =>
            {
                PushPackages(option.Key, "https://f.feedz.io/miru/miru/nuget");
            });
            
            Target("pack-quick", () =>
            {
                foreach (var releasable in Packages)
                {
                    Run("dotnet", $"pack src\\{releasable.Key} -c {buildConfig} --nologo");
                }
            });
            
            Target("publish-nuget", DependsOn("compile", "pack"), () =>
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
                Run("dotnet", "test", workingDirectory: @"samples/Mong/tests/Mong.Tests");
            });
            
            Target("mong-test-all", () =>
            {
                Run("dotnet", "test", workingDirectory: @"samples/Mong");
            });
            
            Target("export-stubs", ExportStubs.Export);
            
            Target("test-new-solution", TestNewSolution.Test);
            
            Target("deep-clean", DeepClean);
            
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
            
            foreach (var (package, withSymbols) in Packages)
            {
                Run("dotnet", $"nuget push {Path.Combine("packages", package)}.*.nupkg {param}", noEcho: true);
            }
        }
    }
}
