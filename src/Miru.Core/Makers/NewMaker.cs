using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Miru.Core.Makers
{
    public static class NewMaker
    {
        public static void New(this Maker m, string name)
        {
            var newSolutionDir = A.Path / m.Solution.RootDir / name;
 
            ThrowIfNewDirectoryExist(newSolutionDir);
            
            Console.WriteLine();
            
            // root
            m.Directory();
            Console.WriteLine();
            
            var map = Yml.FromYml<Dictionary<string, string>>(Maker.ReadEmbedded("_New.yml"));

            foreach (var (key, stub) in map)
            {
                // FIXME: Use same tokens replacement as m.Template
                var destination = key
                    .Replace("Corpo.Skeleton", m.Solution.Name)
                    .Replace("Skeleton", m.Solution.ShortName)
                    .Replace('\\', Path.DirectorySeparatorChar);
                
                m.Template(stub, destination);
            }
            
            m.Template(
                "appSettings-example.yml", 
                new { environment = "Development"}, 
                A.Path / "src" / name / "appSettings.Development.yml");
            
            m.Template(
                "appSettings-example.yml", 
                new { environment = "Test"}, 
                A.Path / "src" / name / "appSettings.Test.yml");
            
            Console2.BreakLine();
            Console2.Line($"New solution created at:");
            Console2.BreakLine();
            Console2.GreenLine($"\t{m.Solution.RootDir}");
            Console2.BreakLine();
            Console2.GreenLine("Good luck!");
        }

        private static void ThrowIfNewDirectoryExist(MiruPath newSolutionDir)
        {
            if (Directory.Exists(newSolutionDir))
                throw new MakeException(
                    $"Can't create new Miru solution. Directory {newSolutionDir} already exist");
        }
    }
}