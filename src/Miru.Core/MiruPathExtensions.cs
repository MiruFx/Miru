using System.IO;

namespace Miru.Core
{
    public static class MiruPathExtensions
    {
        public static bool Exists(this MiruPath path) => Directory.Exists(path);
        
        public static string Relative(this MiruPath current, string path) => Path.GetRelativePath(current, path);
    }
}