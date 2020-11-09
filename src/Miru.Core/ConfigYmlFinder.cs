using System.IO;
using Baseline;

namespace Miru.Core
{
    public class ConfigYmlFinder
    {
        private readonly IFileSystem _fs;

        public ConfigYmlFinder() : this(new FileSystem())
        {
        }
        
        public ConfigYmlFinder(IFileSystem fs)
        {
            _fs = fs;
        }

        public string FindConfigYmlFromDir(string fromDir, string environment, MiruSolution solution)
        {
            var travelDir = new DirectoryInfo(fromDir);

            while (travelDir != null)
            {
                // when reaches Solution.RootDir, use ConfigDir
                if (travelDir.FullName.Equals(solution.RootDir))
                    return A.Path(solution.ConfigDir, MiruSolution.ConfigYml(environment));
                
                var file = A.Path(travelDir.FullName, MiruSolution.ConfigYml(environment));
                
                if (_fs.FileExists(file))
                    return file;

                travelDir = travelDir.Parent;
            }

            return string.Empty;
        }
    }
}