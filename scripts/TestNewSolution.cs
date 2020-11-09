using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Baseline;
using Miru;
using Miru.Core;

namespace Scripts
{
    public class TestNewSolution
    {
        public static void Test()
        {
            var tempDir = A.TempPath() / "MiruTemp";
            var solutionDir = tempDir / "Supportreon";

            Directories.DeleteIfExists(tempDir);
            Directory.CreateDirectory(tempDir);
            
            OS.ShellToConsole("miru new Supportreon", tempDir);
            OS.ShellToConsole("dotnet build", solutionDir);
            OS.ShellToConsole("miru @app npm install", solutionDir);
            OS.ShellToConsole("miru @app npm run dev", solutionDir);
            
            OS.ShellToConsole("miru make:migration CreateProjects", solutionDir);
            OS.ShellToConsole("miru make:entity Project", solutionDir);
            OS.ShellToConsole("miru make:feature Projects Project New --new", solutionDir);
            OS.ShellToConsole("miru make:feature Projects Project Edit --edit", solutionDir);
            OS.ShellToConsole("miru make:feature Projects Project List --list", solutionDir);
            OS.ShellToConsole("miru make:feature Projects Project Show --show", solutionDir);
            
            EditMigrationCreateProject(solutionDir);
            EditDbContext(solutionDir);
            
            OS.ShellToConsole("dotnet build", solutionDir);
            OS.ShellToConsole("miru db:migrate", solutionDir);
            OS.ShellToConsole("dotnet test", solutionDir);
        }

        private static void EditDbContext(MiruPath solutionDir)
        {
            ReplaceFileContent(
                solutionDir / "src" / "Supportreon" / "Database" / "SupportreonDbContext.cs", 
                "// Your entities", 
                $"// Your entities{Environment.NewLine}\t\tpublic DbSet<Project> Projects {{ get; set; }}");
        }

        private static void EditMigrationCreateProject(MiruPath solutionDir)
        {
            var file = FindFile(solutionDir / "src" / "Supportreon" / "Database" / "Migrations", "*_CreateProjects.cs");

            ReplaceFileContent(file, "TableName", "Projects");
        }

        private static void ReplaceFileContent(string file, string content, string replace)
        {
            var fileContent = File.ReadAllText(file);
            
            fileContent = fileContent.Replace(content, replace);
            
            File.WriteAllText(file, fileContent);
        }

        private static string FindFile(string path, string pattern)
        {
            var files = Directory.EnumerateFiles(path, pattern);

            if (files.Count() != 1)
                throw new InvalidOperationException($"More than one {pattern} found");
            
            return files.At(0);
        }
    }
}