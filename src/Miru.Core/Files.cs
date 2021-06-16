using System.IO;

namespace Miru.Core
{
    public static class Files
    {
        /// <summary>
        /// Create a file with given content. File's directory will be created if it does not exist
        /// </summary>
        public static void Create(string file, string content)
        {
            Directories.CreateIfNotExists(Path.GetDirectoryName(file));
            
            using (var writer = File.CreateText(file))
            {
                writer.Write(content);
            }
        }

        public static void DeleteIfExists(string file)
        {
            if (File.Exists(file)) 
                File.Delete(file);
        }
    }
}