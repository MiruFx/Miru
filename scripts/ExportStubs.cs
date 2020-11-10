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
            _dir = _rootDir / "samples" / "Skeleton";
            _stubDir = _rootDir / "src" / "Miru.Core" / "Templates";
            
            Directories.DeleteIfExists(_stubDir);
            Directory.CreateDirectory(_stubDir);
            
            // New
            ExportFile(_dir / "Skeleton.sln", "Solution.sln");
            ExportFile(_dir / "gitignore", ".gitignore", destinationFile: ".gitignore");
            ExportFile(_dir / "global.json");
            
            ExportDir(_dir / "config");
            
            ExportFile(_dir / "src" / "Skeleton" / "webpack.mix.js");
            ExportFile(_dir / "src" / "Skeleton" / "Startup.cs");
            ExportFile(_dir / "src" / "Skeleton" / "Skeleton.csproj");
            ExportFile(_dir / "src" / "Skeleton" / "Program.cs");
            ExportFile(_dir / "src" / "Skeleton" / "package.json");

            ExportDir(_dir / "src" / "Skeleton" / "Config");
            ExportFile(_dir / "src" / "Skeleton" / "Database" / "Migrations" / "202006101850_CreateUsers.cs");
            ExportFile(_dir / "src" / "Skeleton" / "Database" / "SkeletonDbContext.cs");
            ExportFile(_dir / "src" / "Skeleton" / "Domain" / "User.cs");
            
            ExportFile(_dir / "src" / "Skeleton" / "Features" / "_ViewImports.cshtml");
            ExportFile(_dir / "src" / "Skeleton" / "Features" / "_ViewStart.cshtml");
            ExportDir(_dir / "src" / "Skeleton" / "Features" / "Accounts");
            ExportDir(_dir / "src" / "Skeleton" / "Features" / "Home");
            ExportDir(_dir / "src" / "Skeleton" / "Features" / "Shared");
            ExportDir(_dir / "src" / "Skeleton" / "resources");
            
            ExportFile(_dir / "tests" / "Skeleton.Tests" / "Skeleton.Tests.csproj");
            ExportFile(_dir / "tests" / "Skeleton.Tests" / "SkeletonFabricator.cs");
            ExportFile(_dir / "tests" / "Skeleton.Tests" / "Program.cs", "'Program");
            ExportFile(_dir / "tests" / "Skeleton.Tests" / "Extensions.cs");
            ExportDir(_dir / "tests" / "Skeleton.Tests" / "Features" / "Accounts");
            ExportDir(_dir / "tests" / "Skeleton.Tests" / "Features" / "Home");
            ExportDir(_dir / "tests" / "Skeleton.Tests" / "Config");
            
            ExportFile(_dir / "tests" / "Skeleton.PageTests" / "Skeleton.PageTests.csproj");
            ExportFile(_dir / "tests" / "Skeleton.PageTests" / "Program.cs", "''Program");
            ExportDir(_dir / "tests" / "Skeleton.PageTests" / "Pages" / "Accounts");
            ExportDir(_dir / "tests" / "Skeleton.PageTests" / "Pages" / "Home");
            ExportDir(_dir / "tests" / "Skeleton.PageTests" / "Config");
            
            SaveMapForNewSolution();
            
            // Command
            ExportFile(_dir / "src" / "Skeleton" / "Features" / "Products" / "ProductEdit.cs", "Command", "Edit");
            ExportFile(_dir / "src" / "Skeleton" / "Features" / "Products" / "Edit.cshtml", "Command.cshtml", "Edit");
            ExportFile(_dir / "src" / "Skeleton" / "Features" / "Products" / "_Edit.js.cshtml", "_Command.js.cshtml", "Edit");
            ExportFile(_dir / "tests" / "Skeleton.Tests" / "Features" / "Products" / "ProductEditTest.cs", "CommandTest", "Edit");
            ExportFile(_dir / "tests" / "Skeleton.PageTests" / "Pages" / "Products" / "ProductEditPageTest.cs", "CommandPageTest", "Edit");
            
            // Query
            ExportFile(_dir / "src" / "Skeleton" / "Features" / "Products" / "ProductList.cs", "Query", "List");
            ExportFile(_dir / "src" / "Skeleton" / "Features" / "Products" / "List.cshtml", "Query.cshtml", "List");
            ExportFile(_dir / "tests" / "Skeleton.Tests" / "Features" / "Products" / "ProductListTest.cs", "QueryTest", "List");
            ExportFile(_dir / "tests" / "Skeleton.PageTests" / "Pages" / "Products" / "ProductListPageTest.cs", "QueryPageTest", "List");
            
            // Migration
            ExportFile(_dir / "src" / "Skeleton" / "Database" / "Migrations" / "999999999999_CreateCards.cs", "Migration");
            
            // Entity
            ExportFile(_dir / "src" / "Skeleton" / "Domain" / "Product.cs", "Entity");
            ExportFile(_dir / "tests" / "Skeleton.Tests" / "Domain" / "ProductTest.cs", "EntityTest");
            
            // Consolable
            ExportFile(_dir / "src" / "Skeleton" / "Consolables" / "SeedConsolable.cs", "Consolable");
            
            // Job
            ExportFile(_dir / "src" / "Skeleton" / "Features" / "Products" / "ProductCreated.cs", "Job", templateKey: "Job");
            ExportFile(_dir / "tests" / "Skeleton.Tests" / "Features" / "Products" / "ProductCreatedTest.cs", "JobTest", templateKey: "Job");
            
            // Email
            ExportFile(_dir / "src" / "Skeleton" / "Features" / "Products" / "ProductCreatedMail.cs", "Mailable", templateKey: "Email");
            ExportFile(_dir / "src" / "Skeleton" / "Features" / "Products" / "ProductCreatedMail.cshtml", "MailTemplate", templateKey: "Email");

            // Config
            ExportFile(_dir / "config" / "_Config.Example.yml", "Config");
            
            // Feature-New
            ExportFile(_dir / "src" / "Skeleton" / "Features" / "Products" / "ProductNew.cs", "New-Feature");
            ExportFile(_dir / "src" / "Skeleton" / "Features" / "Products" / "New.cshtml", "New-Feature.cshtml");
            ExportFile(_dir / "src" / "Skeleton" / "Features" / "Products" / "_New.js.cshtml", "New-_Feature.js.cshtml");
            ExportFile(_dir / "tests" / "Skeleton.Tests" / "Features" / "Products" / "ProductNewTest.cs", "New-FeatureTest");
            ExportFile(_dir / "tests" / "Skeleton.PageTests" / "Pages" / "Products" / "ProductNewPageTest.cs", "New-FeaturePageTest");
            
            // Feature-Edit
            ExportFile(_dir / "src" / "Skeleton" / "Features" / "Products" / "ProductEdit.cs", "Edit-Feature", templateKey: "Edit");
            ExportFile(_dir / "src" / "Skeleton" / "Features" / "Products" / "Edit.cshtml", "Edit-Feature.cshtml", templateKey: "Edit");
            ExportFile(_dir / "src" / "Skeleton" / "Features" / "Products" / "_Edit.js.cshtml", "Edit-_Feature.js.cshtml", templateKey: "Edit");
            ExportFile(_dir / "tests" / "Skeleton.Tests" / "Features" / "Products" / "ProductEditTest.cs", "Edit-FeatureTest", templateKey: "Edit");
            ExportFile(_dir / "tests" / "Skeleton.PageTests" / "Pages" / "Products" / "ProductEditPageTest.cs", "Edit-FeaturePageTest", templateKey: "Edit");
            
            // Feature-Show
            ExportFile(_dir / "src" / "Skeleton" / "Features" / "Products" / "ProductShow.cs", "Show-Feature", templateKey: "Show");
            ExportFile(_dir / "src" / "Skeleton" / "Features" / "Products" / "Show.cshtml", "Show-Feature.cshtml", templateKey: "Show");
            ExportFile(_dir / "tests" / "Skeleton.Tests" / "Features" / "Products" / "ProductShowTest.cs", "Show-FeatureTest", templateKey: "Show");
            ExportFile(_dir / "tests" / "Skeleton.PageTests" / "Pages" / "Products" / "ProductShowPageTest.cs", "Show-FeaturePageTest", templateKey: "Show");
            
            // Feature-List
            ExportFile(_dir / "src" / "Skeleton" / "Features" / "Products" / "ProductList.cs", "List-Feature", templateKey: "List");
            ExportFile(_dir / "src" / "Skeleton" / "Features" / "Products" / "List.cshtml", "List-Feature.cshtml", templateKey: "List");
            ExportFile(_dir / "tests" / "Skeleton.Tests" / "Features" / "Products" / "ProductListTest.cs", "List-FeatureTest", templateKey: "List");
            ExportFile(_dir / "tests" / "Skeleton.PageTests" / "Pages" / "Products" / "ProductListPageTest.cs", "List-FeaturePageTest", templateKey: "List");
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
                .Replace("public DbSet<Product> Products { get; set; }", "// public DbSet<Product> Products { get; set; }")
                .Replace("@using Skeleton.Features.Products", string.Empty)
                .Replace("Skeleton", "{{ Solution.Name }}")
                .Replace("Products", "{{ input.In }}")
                .Replace("products", "{{ string.downcase input.In }}")
                .Replace("Product", "{{ input.Name }}")
                .Replace("product", "{{ string.downcase input.Name }}")
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
                .Replace("999999999999", "{{ input.Version }}")
                .Replace("CreateCards", "{{ input.Name }}")
            );

            File.WriteAllLines(stubPath, fileContent);

            if (_map)
                NewSolutionFiles.Add(Path.GetRelativePath(_dir, destinationFile ?? file), stubFileName);
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