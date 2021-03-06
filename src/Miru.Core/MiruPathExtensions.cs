using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Miru.Core
{
    public static class MiruPathExtensions
    {
        public static bool Exists(this MiruPath path) => Directory.Exists(path);
        
        public static string Relative(this MiruPath current, string path) => Path.GetRelativePath(current, path);
        
        public static void DeleteIfExists(this MiruPath miruPath) => Core.Files.DeleteIfExists(miruPath.ToString());
    
        public static void DeleteDir(this MiruPath miruPath) => Directories.DeleteIfExists(miruPath);

        public static bool Exists2(this MiruPath miruPath) =>
            Directory.Exists(miruPath) || File.Exists(miruPath);

        public static bool DontExistOrEmpty(this MiruPath miruPath) =>
            miruPath.Exists2() == false || miruPath.FileInfo().Length == 0;

        public static void DeleteFileIfEmpty(this MiruPath miruPath)
        {
            if (miruPath.Exists2() && miruPath.FileInfo().Length == 0)
                miruPath.DeleteIfExists();
        }

        public static void EnsureDirExist(this MiruPath miruPath) =>
            Directories.CreateIfNotExists(miruPath.IsDirectory() ? miruPath : miruPath.Dir());
    
        public static void EnsureDirExist2(this MiruPath miruPath) =>
            Directories.CreateIfNotExists(miruPath);
        
        public static FileInfo FileInfo(this MiruPath miruPath) => new FileInfo(miruPath);
    
        public static bool DirContains(this MiruPath miruPath, string searchPattern) =>
            new DirectoryInfo(miruPath).GetFiles(searchPattern).Any();

        public static IEnumerable<FileInfo> Files(this MiruPath miruPath, string searchPattern) =>
            new DirectoryInfo(miruPath).GetFiles(searchPattern);

        public static bool IsDirectory(this MiruPath miruPath) => Directory.Exists(miruPath);

        public static MiruPath Dir(this MiruPath miruPath) => Path.GetDirectoryName(miruPath);
    }
}