using System;
using System.IO;
using System.Reflection;

namespace Miru.Core
{
    public class EmbeddedFiles<TAssemblyOfType>
    {
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