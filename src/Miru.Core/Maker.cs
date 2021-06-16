using System.IO;
using System.Reflection;
using Baseline;
using Scriban;

namespace Miru.Core
{
    public class Maker
    {
        public static Maker For(MiruPath currentDirectory)
        {
            return new Maker(new MiruSolution(currentDirectory));
        }
        
        public static Maker For(MiruPath currentDirectory, string solutionName)
        {
            if (Path.GetFileName(currentDirectory).Equals(solutionName) == false)
                currentDirectory = Path.Combine(currentDirectory, solutionName);
            
            return new Maker(new MiruSolution(currentDirectory));
        }
        
        public MiruSolution Solution { get; }
        
        public Maker(MiruSolution solution)
        {
            Solution = solution;
        }

        public void Directory(params string[] paths)
        {
            var newDir = A.Path / Solution.RootDir / paths;
            var shortDestination = Path.Combine(paths);
            
            System.IO.Directory.CreateDirectory(newDir);
            
            Console2.YellowLine($"\tCreate\t{(paths.Length > 0 ? shortDestination : newDir.ToString())}");
        }

        public void Template(string templateName, params string[] to)
        {
            Template(templateName, new
            {
                Solution,
                MiruInfo.MiruVersion
            }, to);
        }

        public void Template(string templateName, object input, params string[] to)
        {
            var template = GetTemplate(templateName);

            var shortDestination = Path.Combine(to);
            
            var destination = Path.Combine(Solution.RootDir, shortDestination);

            if (File.Exists(destination))
            {
                Console2.GreyLine($"\tSkip\t{Solution.RootDir.Relative(destination)}");
                return;
            }

            Console2.YellowLine($"\tCreate\t{Solution.RootDir.Relative(destination)}");
            
            var result = template.Render(new
            {
                Solution,
                MiruInfo.MiruVersion,
                input
            }, member => member.Name);
            
            Directories.CreateIfNotExists(Path.GetDirectoryName(destination));
            File.AppendAllText(destination, result);
        }

        private Template GetTemplate(string templateName)
        {
            if (!templateName.EndsWith(".stub"))
                templateName += ".stub";

            var templateText = ReadEmbedded(templateName);

            var template = Scriban.Template.Parse(templateText);
            return template;
        }

        public static string ReadEmbedded(string fileName)
        {
            var assembly = typeof(Maker).GetTypeInfo().Assembly;
            var @namespace = typeof(Maker).Namespace;
            var resourceName = $"{@namespace}.Templates.{fileName}";
            
            var stream = assembly.GetManifestResourceStream(resourceName);

            if (stream == null)
                throw new FileNotFoundException($"Could not find the resource: {resourceName}");
            
            using (stream)
            {
                return stream.ReadAllText();
            }
        }

        public MiruPath Expand(string @in)
        {
            return Path.Combine(@in.Split("/"));
        }

        public string Namespace(string @in)
        {
            return @in.Replace('\\', '.').Replace('/', '.');
        }
    }
}