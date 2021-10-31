using System.Collections.Generic;
using System.IO;
using Miru.Core;

namespace Miru.Html;

public class AssetsMap
{
    private readonly Dictionary<string, string> _map;

    public AssetsMap(MiruSolution solution)
    {
        var mixManifestPath = solution.AppDir / "wwwroot" / "mix-manifest.json";

        if (mixManifestPath.FileDoesNotExist())
        {
            throw new MiruException(@$"The file {mixManifestPath} could not be found.

It is used by Miru to reference the correct frontend assets (css, js, images, and etc).

To generate it, run the command 'miru app npm run dev'.

More details at https://mirufx.github.io/Frontend/JavascriptCssAssets.html#bundling");
        }
            
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