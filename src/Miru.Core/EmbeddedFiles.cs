using System;
using System.IO;
using System.Reflection;
using Baseline;

namespace Miru.Core
{
    public class EmbeddedFiles<TAssemblyOfType>
    {
        public static string ReadEmbedded(string fullName)
        {
            var assembly = typeof(TAssemblyOfType).GetTypeInfo().Assembly;
            
            var stream = assembly.GetManifestResourceStream(fullName);

            if (stream == null)
                throw new FileNotFoundException($"Could not find the resource: {fullName}");
            
            using (stream)
            {
                return stream.ReadAllText();
            }
        }
        
        public void ExtractFile(string resource, string destinationPath)
        {
            var assembly = typeof(TAssemblyOfType).GetTypeInfo().Assembly;
            var assemblyName = assembly.GetName().Name;
            
            var stream = assembly.GetManifestResourceStream($"{assemblyName}.{resource}");

            if (stream == null)
            {
                throw new InvalidOperationException($"Could not find {resource} inside {assemblyName}");
            }
            
            if (File.Exists(destinationPath))
                File.Delete(destinationPath);
            
            Directories.CreateIfNotExists(Path.GetDirectoryName(destinationPath));
            
            using (stream)
            {
                using (var fileStream = File.Create(destinationPath))
                {
                    stream.CopyTo(fileStream);
                }
            }
        }
    }
}