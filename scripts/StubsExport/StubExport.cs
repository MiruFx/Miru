using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Baseline;
using Miru.Core;

namespace Scripts.StubsExport;

public abstract class StubExport
{
    protected Dictionary<string, string> NewSolutionFiles = new();

    protected StubParams Params { get; set; }

    protected StubExport(StubParams stubParams)
    {
        Params = stubParams;
    }

    public abstract void Export();
    
    protected void ExportDir(string dir)
    {
        foreach (var file in Directory.EnumerateFiles(dir, "*.*", SearchOption.AllDirectories))
        {
            ExportFile(file);
        }
    }
    
    protected void ExportFile(
            string file, 
            string stub = null, 
            string templateKey = "", 
            string destinationFile = null,
            Func<string, string> tokens = null)
        {
            var stubFileName = BuildStubName(file, stub);
                
            var stubPath = Params.StubDir / stubFileName;
            
            Files.DeleteIfExists(stubPath);
            
            var fileContent = File.ReadAllLines(file).Select(line =>
                {
                    if (tokens != null)
                        line = tokens(line);

                    line = line
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
                        .ReplaceIf(templateKey == "Show", "show", "{{ string.downcase input.Action }}");

                    return line;
                }
            );

            File.WriteAllLines(stubPath, fileContent);

            if (Params.SolutionMap)
                NewSolutionFiles.Add(destinationFile ?? Path.GetRelativePath(Params.SkeletonDir, file), stubFileName);
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