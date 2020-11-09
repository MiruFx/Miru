using System.Collections.Generic;
using System.IO;
using Miru.Core;

namespace Miru.Html
{
    public class AssetsMap
    {
        private readonly Dictionary<string, string> _map;

        public AssetsMap(MiruSolution solution)
        {
            var mixManifestPath = solution.AppDir / "wwwroot" / "mix-manifest.json";
            
            var mixManifest = File.ReadAllText(mixManifestPath);
            
            _map = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(mixManifest);
        }

        public string For(string path)
        {
            if (_map.TryGetValue(path, out string mixedPath))
                return mixedPath;

            return path;
        }
    }
}