using System.IO;

namespace Miru.Core
{
    public static class Directories
    {
        public static void CreateIfNotExists(string directoryName)
        {
            if (Directory.Exists(directoryName) == false)
                Directory.CreateDirectory(directoryName);
        }

        public static void DeleteIfExists(string directoryName)
        {
            if (Directory.Exists(directoryName))
                Directory.Delete(directoryName, true);
        }

        public static void CreateForPathIfNotExists(string file)
        {
            var directory = Path.GetDirectoryName(file);
            
            CreateIfNotExists(directory);
        }
    }
}