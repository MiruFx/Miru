using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Baseline;
using Miru;
using Miru.Core;

namespace Scripts
{
    public class ExportStubs
    {
        private static readonly Dictionary<string, string> NewSolutionFiles = new Dictionary<string, string>();
        private static MiruPath _rootDir;
        private static MiruPath _dir;
        private static MiruPath _stubDir;
        private static bool _map = true;

        public static void Export()
        {
            _rootDir = new SolutionFinder().FromCurrentDir().Solution.RootDir;
            _dir = _rootDir / "samples" / "Corpo.Skeleton";
            _stubDir = _rootDir / "src" / "Miru.Core" / "Templates";
            
            Directories.DeleteIfExists(_stubDir);
            Directory.CreateDirectory(_stubDir);
            
            // FIXME: use Solution to build the artifacts' path
            
            // New
            ExportFile(_rootDir / "global.json", destinationFile: "global.json");
            ExportFile(_dir / "Corpo.Skeleton.sln", "Solution.sln");
            ExportFile(_dir / "gitignore", ".gitignore", destinationFile: ".gitignore");
            ExportFile(_dir / "Readme.md");
            
            ExportDir(_dir / "config");
            
            ExportFile(_dir / "src" / "Corpo.Skeleton" / "webpack.mix.js");
            ExportFile(_dir / "src" / "Corpo.Skeleton" / "Startup.cs");
            ExportFile(_dir / "src" / "Corpo.Skeleton" / "Corpo.Skeleton.csproj");
            ExportFile(_dir / "src" / "Corpo.Skeleton" / "Program.cs");
            ExportFile(_dir / "src" / "Corpo.Skeleton" / "package.json");

            ExportDir(_dir / "src" / "Corpo.Skeleton" / "Config");
            ExportFile(_dir / "src" / "Corpo.Skeleton" / "Database" / "Migrations" / "202001290120_CreateUserfy.cs");
            ExportFile(_dir / "src" / "Corpo.Skeleton" / "Database" / "SkeletonDbContext.cs");
            ExportFile(_dir / "src" / "Corpo.Skeleton" / "Domain" / "User.cs");
            
            ExportFile(_dir / "src" / "Corpo.Skeleton" / "Features" / "_ViewImports.cshtml");
            ExportFile(_dir / "src" / "Corpo.Skeleton" / "Features" / "_ViewStart.cshtml");
            ExportDir(_dir / "src" / "Corpo.Skeleton" / "Features" / "Accounts");
            ExportDir(_dir / "src" / "Corpo.Skeleton" / "Features" / "Home");
            ExportDir(_dir / "src" / "Corpo.Skeleton" / "Features" / "Shared");
            ExportDir(_dir / "src" / "Corpo.Skeleton" / "resources");
            
            ExportFile(_dir / "tests" / "Corpo.Skeleton.Tests" / "Corpo.Skeleton.Tests.csproj");
            ExportFile(_dir / "tests" / "Corpo.Skeleton.Tests" / "SkeletonFabricator.cs");
            ExportFile(_dir / "tests" / "Corpo.Skeleton.Tests" / "Program.cs", "'Program");
            ExportFile(_dir / "tests" / "Corpo.Skeleton.Tests" / "Extensions.cs");
            ExportDir(_dir / "tests" / "Corpo.Skeleton.Tests" / "Features" / "Accounts");
            ExportDir(_dir / "tests" / "Corpo.Skeleton.Tests" / "Features" / "Home");
            ExportDir(_dir / "tests" / "Corpo.Skeleton.Tests" / "Config");
            
            ExportFile(_dir / "tests" / "Corpo.Skeleton.PageTests" / "Corpo.Skeleton.PageTests.csproj");
            ExportFile(_dir / "tests" / "Corpo.Skeleton.PageTests" / "Program.cs", "''Program");
            ExportDir(_dir / "tests" / "Corpo.Skeleton.PageTests" / "Pages" / "Accounts");
            ExportDir(_dir / "tests" / "Corpo.Skeleton.PageTests" / "Pages" / "Home");
            ExportDir(_dir / "tests" / "Corpo.Skeleton.PageTests" / "Config");
            
            SaveMapForNewSolution();
            
            // Command
            ExportFile(_dir / "src" / "Corpo.Skeleton" / "Features" / "Teams" / "TeamEdit.cs", "Command", "Edit");
            ExportFile(_dir / "src" / "Corpo.Skeleton" / "Features" / "Teams" / "Edit.cshtml", "Command.cshtml", "Edit");
            ExportFile(_dir / "src" / "Corpo.Skeleton" / "Features" / "Teams" / "_Edit.js.cshtml", "_Command.js.cshtml", "Edit");
            ExportFile(_dir / "tests" / "Corpo.Skeleton.Tests" / "Features" / "Teams" / "TeamEditTest.cs", "CommandTest", "Edit");
            ExportFile(_dir / "tests" / "Corpo.Skeleton.PageTests" / "Pages" / "Teams" / "TeamEditPageTest.cs", "CommandPageTest", "Edit");
            
            // Query
            ExportFile(_dir / "src" / "Corpo.Skeleton" / "Features" / "Teams" / "TeamList.cs", "Query", "List");
            ExportFile(_dir / "src" / "Corpo.Skeleton" / "Features" / "Teams" / "List.cshtml", "Query.cshtml", "List");
            ExportFile(_dir / "tests" / "Corpo.Skeleton.Tests" / "Features" / "Teams" / "TeamListTest.cs", "QueryTest", "List");
            ExportFile(_dir / "tests" / "Corpo.Skeleton.PageTests" / "Pages" / "Teams" / "TeamListPageTest.cs", "QueryPageTest", "List");
            
            // Migration
            ExportFile(_dir / "src" / "Corpo.Skeleton" / "Database" / "Migrations" / "999999999999_CreateCards.cs", "Migration");
            
            // Entity
            ExportFile(_dir / "src" / "Corpo.Skeleton" / "Domain" / "Team.cs", "Entity");
            ExportFile(_dir / "tests" / "Corpo.Skeleton.Tests" / "Domain" / "TeamTest.cs", "EntityTest");
            
            // Consolable
            ExportFile(_dir / "src" / "Corpo.Skeleton" / "Consolables" / "SeedConsolable.cs", "Consolable");
            
            // Job
            ExportFile(_dir / "src" / "Corpo.Skeleton" / "Features" / "Teams" / "TeamCreated.cs", "Job", templateKey: "Job");
            ExportFile(_dir / "tests" / "Corpo.Skeleton.Tests" / "Features" / "Teams" / "TeamCreatedTest.cs", "JobTest", templateKey: "Job");
            
            // Email
            ExportFile(_dir / "src" / "Corpo.Skeleton" / "Features" / "Teams" / "TeamCreatedMail.cs", "Mailable", templateKey: "Email");
            ExportFile(_dir / "src" / "Corpo.Skeleton" / "Features" / "Teams" / "_Created.mail.cshtml", "MailTemplate", templateKey: "Email");

            // Config
            ExportFile(_dir / "config" / "_Config.Example.yml", "Config");
            ExportFile(_dir / "config" / "_Config.Example.yml", "Config", destinationFile: "Config.Development.yml");
            ExportFile(_dir / "config" / "_Config.Example.yml", "Config", destinationFile: "Config.Test.yml");
            ExportFile(_dir / "config" / "_Config.Example.yml", "Config", destinationFile: "Config.Production.yml");
            
            // Feature-New
            ExportFile(_dir / "src" / "Corpo.Skeleton" / "Features" / "Categories" / "CategoryNew.cs", "New-Feature");
            ExportFile(_dir / "src" / "Corpo.Skeleton" / "Features" / "Categories" / "New.cshtml", "New-Feature.cshtml");
            ExportFile(_dir / "src" / "Corpo.Skeleton" / "Features" / "Categories" / "_New.js.cshtml", "New-_Feature.js.cshtml");
            ExportFile(_dir / "tests" / "Corpo.Skeleton.Tests" / "Features" / "Categories" / "CategoryNewTest.cs", "New-FeatureTest");
            ExportFile(_dir / "tests" / "Corpo.Skeleton.PageTests" / "Pages" / "Categories" / "CategoryNewPageTest.cs", "New-FeaturePageTest");
            
            // Feature-Edit
            ExportFile(_dir / "src" / "Corpo.Skeleton" / "Features" / "Categories" / "CategoryEdit.cs", "Edit-Feature", templateKey: "Edit");
            ExportFile(_dir / "src" / "Corpo.Skeleton" / "Features" / "Categories" / "Edit.cshtml", "Edit-Feature.cshtml", templateKey: "Edit");
            ExportFile(_dir / "src" / "Corpo.Skeleton" / "Features" / "Categories" / "_Edit.js.cshtml", "Edit-_Feature.js.cshtml", templateKey: "Edit");
            ExportFile(_dir / "tests" / "Corpo.Skeleton.Tests" / "Features" / "Categories" / "CategoryEditTest.cs", "Edit-FeatureTest", templateKey: "Edit");
            ExportFile(_dir / "tests" / "Corpo.Skeleton.PageTests" / "Pages" / "Categories" / "CategoryEditPageTest.cs", "Edit-FeaturePageTest", templateKey: "Edit");
            
            // Feature-Show
            ExportFile(_dir / "src" / "Corpo.Skeleton" / "Features" / "Categories" / "CategoryShow.cs", "Show-Feature", templateKey: "Show");
            ExportFile(_dir / "src" / "Corpo.Skeleton" / "Features" / "Categories" / "Show.cshtml", "Show-Feature.cshtml", templateKey: "Show");
            ExportFile(_dir / "tests" / "Corpo.Skeleton.Tests" / "Features" / "Categories" / "CategoryShowTest.cs", "Show-FeatureTest", templateKey: "Show");
            ExportFile(_dir / "tests" / "Corpo.Skeleton.PageTests" / "Pages" / "Categories" / "CategoryShowPageTest.cs", "Show-FeaturePageTest", templateKey: "Show");
            
            // Feature-List
            ExportFile(_dir / "src" / "Corpo.Skeleton" / "Features" / "Categories" / "CategoryList.cs", "List-Feature", templateKey: "List");
            ExportFile(_dir / "src" / "Corpo.Skeleton" / "Features" / "Categories" / "List.cshtml", "List-Feature.cshtml", templateKey: "List");
            ExportFile(_dir / "tests" / "Corpo.Skeleton.Tests" / "Features" / "Categories" / "CategoryListTest.cs", "List-FeatureTest", templateKey: "List");
            ExportFile(_dir / "tests" / "Corpo.Skeleton.PageTests" / "Pages" / "Categories" / "CategoryListPageTest.cs", "List-FeaturePageTest", templateKey: "List");
            
            // Feature-Crud
            ExportFile(_dir / "src" / "Corpo.Skeleton" / "Features" / "Teams" / "TeamNew.cs", "Crud-New-Feature");
            ExportFile(_dir / "src" / "Corpo.Skeleton" / "Features" / "Teams" / "New.cshtml", "Crud-New-Feature.cshtml");
            ExportFile(_dir / "src" / "Corpo.Skeleton" / "Features" / "Teams" / "_New.js.cshtml", "Crud-New-_Feature.js.cshtml");
            ExportFile(_dir / "tests" / "Corpo.Skeleton.Tests" / "Features" / "Teams" / "TeamNewTest.cs", "Crud-New-FeatureTest");
            ExportFile(_dir / "tests" / "Corpo.Skeleton.PageTests" / "Pages" / "Teams" / "TeamNewPageTest.cs", "Crud-New-FeaturePageTest");
            
            ExportFile(_dir / "src" / "Corpo.Skeleton" / "Features" / "Teams" / "TeamEdit.cs", "Crud-Edit-Feature", templateKey: "Edit");
            ExportFile(_dir / "src" / "Corpo.Skeleton" / "Features" / "Teams" / "Edit.cshtml", "Crud-Edit-Feature.cshtml", templateKey: "Edit");
            ExportFile(_dir / "src" / "Corpo.Skeleton" / "Features" / "Teams" / "_Edit.js.cshtml", "Crud-Edit-_Feature.js.cshtml", templateKey: "Edit");
            ExportFile(_dir / "tests" / "Corpo.Skeleton.Tests" / "Features" / "Teams" / "TeamEditTest.cs", "Crud-Edit-FeatureTest", templateKey: "Edit");
            ExportFile(_dir / "tests" / "Corpo.Skeleton.PageTests" / "Pages" / "Teams" / "TeamEditPageTest.cs", "Crud-Edit-FeaturePageTest", templateKey: "Edit");
            
            ExportFile(_dir / "src" / "Corpo.Skeleton" / "Features" / "Teams" / "TeamShow.cs", "Crud-Show-Feature", templateKey: "Show");
            ExportFile(_dir / "src" / "Corpo.Skeleton" / "Features" / "Teams" / "Show.cshtml", "Crud-Show-Feature.cshtml", templateKey: "Show");
            ExportFile(_dir / "tests" / "Corpo.Skeleton.Tests" / "Features" / "Teams" / "TeamShowTest.cs", "Crud-Show-FeatureTest", templateKey: "Show");
            ExportFile(_dir / "tests" / "Corpo.Skeleton.PageTests" / "Pages" / "Teams" / "TeamShowPageTest.cs", "Crud-Show-FeaturePageTest", templateKey: "Show");
            
            ExportFile(_dir / "src" / "Corpo.Skeleton" / "Features" / "Teams" / "TeamList.cs", "Crud-List-Feature", templateKey: "List");
            ExportFile(_dir / "src" / "Corpo.Skeleton" / "Features" / "Teams" / "List.cshtml", "Crud-List-Feature.cshtml", templateKey: "List");
            ExportFile(_dir / "tests" / "Corpo.Skeleton.Tests" / "Features" / "Teams" / "TeamListTest.cs", "Crud-List-FeatureTest", templateKey: "List");
            ExportFile(_dir / "tests" / "Corpo.Skeleton.PageTests" / "Pages" / "Teams" / "TeamListPageTest.cs", "Crud-List-FeaturePageTest", templateKey: "List");
        }

        private static void SaveMapForNewSolution()
        {
            File.WriteAllText(_stubDir / "_New.yml", NewSolutionFiles.ToYml());
            
            NewSolutionFiles.Clear();
            
            _map = false;
        }

        private static void ExportDir(string dir)
        {
            foreach (var file in Directory.EnumerateFiles(dir, "*.*", SearchOption.AllDirectories))
            {
                ExportFile(file);
            }
        }
        
        private static void ExportFile(string file, string stub = null, string templateKey = "", string destinationFile = null)
        {
            var stubFileName = BuildStubName(file, stub);
                
            var stubPath = _stubDir / stubFileName;
            
            Files.DeleteIfExists(stubPath);
            
            var fileContent = File.ReadAllLines(file).Select(s => s
                
                // The order of replaces should be as it is here
                .Replace("{{", "{%{{{")
                .Replace("}}", "}}}%}")
                .Replace(
                    @"<ProjectReference Include=""..\..\..\..\src\Miru\Miru.csproj"" />",
                    @"<PackageReference Include=""Miru"" Version=""{{ MiruVersion }}"" />")
                .Replace(
                    @"<ProjectReference Include=""..\..\..\..\src\Miru.Testing\Miru.Testing.csproj"" />",
                    @"<PackageReference Include=""Miru.Testing"" Version=""{{ MiruVersion }}"" />")
                .Replace(
                    @"<ProjectReference Include=""..\..\..\..\src\Miru.Fabrication\Miru.Fabrication.csproj"" />",
                    @"<PackageReference Include=""Miru.Fabrication"" Version=""{{ MiruVersion }}"" />")
                .Replace(
                    @"<ProjectReference Include=""..\..\..\..\src\Miru.PageTesting\Miru.PageTesting.csproj"" />",
                    @"<PackageReference Include=""Miru.PageTesting"" Version=""{{ MiruVersion }}"" />")
                .Replace(
                    @"<ProjectReference Include=""..\..\..\..\src\Miru.PageTesting.Chrome\Miru.PageTesting.Chrome.csproj"" />",
                    @"<PackageReference Include=""Miru.PageTesting.Chrome"" Version=""{{ MiruVersion }}"" />")
                .Replace(
                    @"<ProjectReference Include=""..\..\..\..\src\Miru.Sqlite\Miru.Sqlite.csproj"" />",
                    @"<PackageReference Include=""Miru.Sqlite"" Version=""{{ MiruVersion }}"" />")
                .Replace(
                    @"<ProjectReference Include=""..\..\..\..\src\Miru.SqlServer\Miru.SqlServer.csproj"" />",
                    @"<PackageReference Include=""Miru.SqlServer"" Version=""{{ MiruVersion }}"" />")
                
                .Replace("Corpo.Skeleton", "{{ Solution.Name }}")
                .Replace("Skeleton", "{{ Solution.ShortName }}")
                
                .Replace("public DbSet<Team> Teams { get; set; }", "// public DbSet<Team> Teams { get; set; }")
                .Replace("Teams", "{{ input.In }}")
                .Replace("teams", "{{ string.downcase input.In }}")
                .Replace("Team", "{{ input.Name }}")
                .Replace("team", "{{ string.downcase input.Name }}")
                .Replace("@using Corpo.Skeleton.Features.Teams", string.Empty)
                
                .Replace("public DbSet<Category> Categories { get; set; }", string.Empty)
                .Replace("Categories", "{{ input.In }}")
                .Replace("categories", "{{ string.downcase input.In }}")
                .Replace("Category", "{{ input.Name }}")
                .Replace("category", "{{ string.downcase input.Name }}")
                .Replace("@using Corpo.Skeleton.Features.Category", string.Empty)
                
                .Replace("Seed", "{{ input.Name }}")
                .Replace("seed", "{{ string.downcase input.Name }}")
                .ReplaceIf(templateKey == "New", "New", "{{ input.Action }}")
                .ReplaceIf(templateKey == "New", "new", "{{ string.downcase input.Action }}")
                .ReplaceIf(templateKey == "Job", "Created", "{{ input.Action }}")
                .ReplaceIf(templateKey == "Job", "created", "{{ string.downcase input.Action }}")
                .ReplaceIf(templateKey == "Email", "Created", "{{ input.Action }}")
                .ReplaceIf(templateKey == "Email", "created", "{{ string.downcase input.Action }}")
                .ReplaceIf(templateKey == "Edit", "Edit", "{{ input.Action }}")
                .ReplaceIf(templateKey == "Edit", "edit", "{{ string.downcase input.Action }}")
                .ReplaceIf(templateKey == "List", "List", "{{ input.Action }}")
                .ReplaceIf(templateKey == "List", "list", "{{ string.downcase input.Action }}")
                .ReplaceIf(templateKey == "Show", "Show", "{{ input.Action }}")
                .ReplaceIf(templateKey == "Show", "show", "{{ string.downcase input.Action }}")
                
                // migration
                .Replace("999999999999", "{{ input.Version }}")
                .Replace("CreateCards", "{{ input.Name }}")
                .Replace("TableName", "{{ input.Table }}")
            );

            File.WriteAllLines(stubPath, fileContent);

            if (_map)
                NewSolutionFiles.Add(destinationFile ?? Path.GetRelativePath(_dir, file), stubFileName);
        }

        private static string BuildStubName(string file, string stub)
        {
            string subName;
            
            if (stub.IsEmpty())
            {
                subName = Path.GetExtension(file) == ".cs" ? 
                    Path.GetFileNameWithoutExtension(file) : 
                    Path.GetFileName(file);
            }
            else
            {
                subName = stub;
            }

            return subName + ".stub";
        }
    }

    public static class StringExtensions
    {
        public static string ReplaceIf(this string value, bool condition, string toReplace, string replaceWith)
        {
            if (condition) return value.Replace(toReplace, replaceWith);

            return value;
        }
    }
}