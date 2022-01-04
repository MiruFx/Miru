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
            ExportFile(_dir / "Corpo.Skeleton.sln", "Solution.sln");
            ExportFile(_dir / "gitignore", ".gitignore", destinationFile: ".gitignore");
            ExportFile(_dir / "README.md");
                        
            ExportFile(_dir / "src" / "Corpo.Skeleton" / "webpack.mix.js");
            ExportFile(_dir / "src" / "Corpo.Skeleton" / "Startup.cs");
            ExportFile(_dir / "src" / "Corpo.Skeleton" / "Corpo.Skeleton.csproj");
            ExportFile(_dir / "src" / "Corpo.Skeleton" / "Program.cs");
            ExportFile(_dir / "src" / "Corpo.Skeleton" / "GlobalUsing.cs");
            ExportFile(_dir / "src" / "Corpo.Skeleton" / "package.json");
            ExportFile(_dir / "src" / "Corpo.Skeleton" / "appSettings-example.yml");

            ExportDir(_dir / "src" / "Corpo.Skeleton" / "Config");
            ExportFile(_dir / "src" / "Corpo.Skeleton" / "Database" / "Migrations" / "202001290120_CreateUsers.cs");
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
            ExportFile(_dir / "tests" / "Corpo.Skeleton.Tests" / "GlobalUsing.cs", "'GlobalUsing");
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
            ExportFile(_dir / "src" / "Corpo.Skeleton" / "Features" / "Tickets" / "TicketEdit.cs", "Command", "Edit");
            ExportFile(_dir / "src" / "Corpo.Skeleton" / "Features" / "Tickets" / "Edit.cshtml", "Command.cshtml", "Edit");
            ExportFile(_dir / "src" / "Corpo.Skeleton" / "Features" / "Tickets" / "_Edit.turbo.cshtml", "_Command.turbo.cshtml", "Edit");
            ExportFile(_dir / "tests" / "Corpo.Skeleton.Tests" / "Features" / "Tickets" / "TicketEditTest.cs", "CommandTest", "Edit");
            ExportFile(_dir / "tests" / "Corpo.Skeleton.PageTests" / "Pages" / "Tickets" / "TicketEditPageTest.cs", "CommandPageTest", "Edit");
            
            // Query-Show
            ExportFile(_dir / "src" / "Corpo.Skeleton" / "Features" / "Tickets" / "TicketShow.cs", "Show-Query", "Show");
            ExportFile(_dir / "src" / "Corpo.Skeleton" / "Features" / "Tickets" / "Show.cshtml", "Show-Query.cshtml", "Show");
            ExportFile(_dir / "tests" / "Corpo.Skeleton.Tests" / "Features" / "Tickets" / "TicketShowTest.cs", "Show-QueryTest", "Show");
            ExportFile(_dir / "tests" / "Corpo.Skeleton.PageTests" / "Pages" / "Tickets" / "TicketShowPageTest.cs", "Show-QueryPageTest", "Show");
            
            // Query-List
            ExportQueryList();

            // TODO: put together the templates file declaration and tokens replacement
            
            // Migration
            ExportFile(_dir / "src" / "Corpo.Skeleton" / "Database" / "Migrations" / "999999999991_CreateCards.cs", "Migration");
            ExportFile(_dir / "src" / "Corpo.Skeleton" / "Database" / "Migrations" / "999999999992_AlterCardsAddUserId.cs", "MigrationAlter");
            
            // Entity
            ExportFile(_dir / "src" / "Corpo.Skeleton" / "Domain" / "Team.cs", "Entity");
            ExportFile(_dir / "tests" / "Corpo.Skeleton.Tests" / "Domain" / "TeamTest.cs", "EntityTest");
            
            // Consolable
            ExportFile(_dir / "src" / "Corpo.Skeleton" / "Consolables" / "SeedConsolable.cs", "Consolable");
            
            // Job
            ExportFile(_dir / "src" / "Corpo.Skeleton" / "Features" / "Teams" / "TeamCreated.cs", "Job", templateKey: "Job");
            ExportFile(_dir / "tests" / "Corpo.Skeleton.Tests" / "Features" / "Teams" / "TeamCreatedTest.cs", "JobTest", templateKey: "Job");
            
            // Email, Mailable
            ExportFile(_dir / "src" / "Corpo.Skeleton" / "Features" / "Teams" / "TeamCreatedMail.cs", "Mailable", templateKey: "Email");
            ExportFile(_dir / "src" / "Corpo.Skeleton" / "Features" / "Teams" / "_Created.mail.cshtml", "MailTemplate", templateKey: "Email");

            // Config
            ExportFile(_dir / "src" / "Corpo.Skeleton" / "appSettings-example.yml", "AppSettings");
            
            // Feature-Crud
            ExportFile(_dir / "src" / "Corpo.Skeleton" / "Features" / "Teams" / "TeamNew.cs", "Crud-New-Feature");
            
            ExportFile(_dir / "src" / "Corpo.Skeleton" / "Features" / "Teams" / "TeamEdit.cs", "Crud-Edit-Feature", templateKey: "Edit");
            ExportFile(_dir / "src" / "Corpo.Skeleton" / "Features" / "Teams" / "Edit.cshtml", "Crud-Edit-Feature.cshtml", templateKey: "Edit");
            ExportFile(_dir / "tests" / "Corpo.Skeleton.Tests" / "Features" / "Teams" / "TeamEditTest.cs", "Crud-Edit-FeatureTest", templateKey: "Edit");
            ExportFile(_dir / "tests" / "Corpo.Skeleton.PageTests" / "Pages" / "Teams" / "TeamEditPageTest.cs", "Crud-Edit-FeaturePageTest", templateKey: "Edit");
            
            ExportFile(_dir / "src" / "Corpo.Skeleton" / "Features" / "Teams" / "TeamDelete.cs", "Crud-Delete-Feature", templateKey: "Delete");
            ExportFile(_dir / "tests" / "Corpo.Skeleton.Tests" / "Features" / "Teams" / "TeamDeleteTest.cs", "Crud-Delete-FeatureTest", templateKey: "Delete");
            // ExportFile(_dir / "tests" / "Corpo.Skeleton.PageTests" / "Pages" / "Teams" / "TeamDeletePageTest.cs", "Crud-Delete-FeaturePageTest", templateKey: "Delete");

            ExportFile(_dir / "src" / "Corpo.Skeleton" / "Features" / "Teams" / "TeamList.cs", "Crud-List-Feature", templateKey: "List");
            ExportFile(_dir / "src" / "Corpo.Skeleton" / "Features" / "Teams" / "List.cshtml", "Crud-List-Feature.cshtml", templateKey: "List");
            ExportFile(_dir / "tests" / "Corpo.Skeleton.Tests" / "Features" / "Teams" / "TeamListTest.cs", "Crud-List-FeatureTest", templateKey: "List");
            ExportFile(_dir / "tests" / "Corpo.Skeleton.PageTests" / "Pages" / "Teams" / "TeamListPageTest.cs", "Crud-List-FeaturePageTest", templateKey: "List");
        }

        private static void ExportQueryList()
        {
            var featureDir = _dir / "src" / "Corpo.Skeleton" / "Features" / "Tickets";
            var testDir = _dir / "tests" / "Corpo.Skeleton.Tests" / "Features" / "Tickets";
            var pageTestDir = _dir / "tests" / "Corpo.Skeleton.PageTests" / "Pages" / "Tickets";
            
            ExportFile(featureDir / "TicketList.cs", "List-Query", "List");
            ExportFile(featureDir / "List.cshtml", "List-Query.cshtml", "List");
            
            ExportFile(testDir / "TicketListTest.cs", "List-QueryTest", "List");
            ExportFile(pageTestDir / "TicketListPageTest.cs", "List-QueryPageTest", "List");
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
                .Replace("EnvironmentName", "{{ input.environment }}")
                
                .Replace("public DbSet<Team> Teams { get; set; }", "// public DbSet<Team> Teams { get; set; }")
                .Replace("Teams", "{{ input.In }}")
                .Replace("teams", "{{ string.downcase input.In }}")
                .Replace("Team", "{{ input.Name }}")
                .Replace("team", "{{ string.downcase input.Name }}")
                .Replace("@using Corpo.Skeleton.Features.Teams", string.Empty)
                
                .Replace("public DbSet<Category> Categories { get; set; }", string.Empty)
                .Replace("[HttpGet(\"/Categories/\"", "{{ input.UrlIn }}")
                .Replace("[HttpPost(\"/Categories/\"", "{{ input.UrlIn }}")
                .Replace("Categories", "{{ input.In }}")
                .Replace("categories", "{{ string.downcase input.In }}")
                .Replace("Category", "{{ input.Name }}")
                .Replace("category", "{{ string.downcase input.Name }}")
                .Replace("@using Corpo.Skeleton.Features.Category", string.Empty)
                
                .Replace("[HttpGet(\"/Tickets/\"", "{{ input.UrlIn }}")
                .Replace("[HttpPost(\"/Tickets/\"", "{{ input.UrlIn }}")
                .Replace("Tickets", "{{ input.In }}")
                .Replace("tickets", "{{ string.downcase input.In }}")
                .Replace("Ticket", "{{ input.Name }}")
                .Replace("ticket", "{{ string.downcase input.Name }}")
                .Replace("@using Corpo.Skeleton.Features.Tickets", string.Empty)                
                
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
                .Replace("999999999991", "{{ input.Version }}")
                .Replace("999999999992", "{{ input.Version }}")
                .Replace("CreateCards", "{{ input.Name }}")
                .Replace("AlterCardsAddUserId", "{{ input.Name }}")
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