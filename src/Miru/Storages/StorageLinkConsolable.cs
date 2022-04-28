using System.IO;
using System.Threading.Tasks;
using Miru.Consolables;
using Miru.Core;

namespace Miru.Storages;

public class StorageLinkConsolable : Consolable
{
    public StorageLinkConsolable() : 
        base("storage.link", "Create a symlink from /storage/assets/public to {App}/wwwroot/public")
    {
    }

    public class ConsolableHandler : IConsolableHandler
    {
        private readonly MiruSolution _solution;

        public ConsolableHandler(MiruSolution solution) => 
            _solution = solution;
            
        public Task Execute()
        {
            var wwwrootPublicDir = _solution.AppDir / "wwwroot" / "public";
            var storagePublicDir = _solution.AssetsDir / "public";

            Console2.GreenLine($"Creating symlink");
            Console2.WhiteLine($"\tfrom: {storagePublicDir}");
            Console2.WhiteLine($"\tto:   {wwwrootPublicDir}");
                
            Directories.CreateIfNotExists(storagePublicDir);

            if (Directory.Exists(wwwrootPublicDir))
            {
                Console2.YellowLine($"{wwwrootPublicDir} already exists... Deleting");
                Directory.Delete(wwwrootPublicDir);
            }
            
            Directory.CreateSymbolicLink(wwwrootPublicDir, storagePublicDir);
            Console2.GreenLine("Link created");
                
            return Task.CompletedTask;
        }
    }
}