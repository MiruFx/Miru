using System.IO;
using Miru.Core;

namespace Miru.Storages
{
    public class Storage
    {
        private readonly MiruSolution _solution;

        public MiruPath StorageDir { get; protected set; }

        public virtual MiruPath App => StorageDir / "app";
        
        public virtual MiruPath Assets => _solution.StorageDir / "assets";

        public Storage(MiruSolution solution)
        {
            _solution = solution;
            
            StorageDir = _solution.StorageDir;
        }

        public MiruPath Temp(params string[] paths)
        {
            var file = _solution.StorageDir / "temp" / Path.Combine(paths);

            CreateDirectoryForPath(file);

            return file;
        }
        
        public MiruPath MakePath(params string[] paths)
        {
            var file = _solution.RootDir / "storage" / Path.Combine(paths);

            CreateDirectoryForPath(file);

            return file;
        }

        private static void CreateDirectoryForPath(MiruPath file)
        {
            var dir = Path.GetDirectoryName(file);
            
            Directories.CreateIfNotExists(dir);
        }
    }
}