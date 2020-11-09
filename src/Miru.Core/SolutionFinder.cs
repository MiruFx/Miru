using System;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using Baseline;

namespace Miru.Core
{
    // TODO: Cache result per path
    public class SolutionFinder
    {
        private readonly IFileSystem _fs;

        public SolutionFinder() : this(new FileSystem())
        {
        }
        
        public SolutionFinder(IFileSystem fs)
        {
            _fs = fs;
        }

        public SolutionFinderResult FromCurrentDir() => FromDir(MiruPath.CurrentPath);
        
        public SolutionFinderResult FromDir(string currentDir)
        {
            var solutionPath = FindSolutionFrom(currentDir);

            if (solutionPath.IsNotEmpty())
            {
                var appName = Path.GetFileNameWithoutExtension(solutionPath);
                var solution = new MiruSolution(Path.GetDirectoryName(solutionPath), appName);
                
                return new SolutionFinderResult(solution);
            }
            
            return SolutionFinderResult.Empty;
        }

        public string FindSolutionFrom(string currentDir)
        {
            return FindClosestOnParents(currentDir, "*.sln");
        }
        
        private string FindClosestOnParents(string currentDir, string pattern)
        {
            var travelDir = new DirectoryInfo(currentDir);

            while (travelDir != null)
            {
                var fileSet = new FileSet()
                {
                    Include = pattern,
                    DeepSearch = false
                };
                
                var files = _fs.FindFiles(travelDir.FullName, fileSet).ToImmutableList();

                if (!files.Any())
                    travelDir = travelDir.Parent;
                else if (files.Count > 1)
                    throw new InvalidOperationException(
                        $"Miru found more than one .sln file at {currentDir}. Only one file is supported. Files found: {files.Join(",")}");
                else
                {
                    var file = Path.GetFileName(files[0]);
                    
                    return Path.Combine(Path.GetDirectoryName(files[0]), file);
                }
            }

            return string.Empty;
        }
    }
}