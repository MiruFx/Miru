using System.IO;
using System.Threading.Tasks;
using Miru.Consolables;
using Miru.Core;

namespace Miru.Storages
{
    public class StorageLinkConsolable : Consolable
    {
        public StorageLinkConsolable() : 
            base("storage.link", "Create a symlink from /storage/assets to {{App}}/wwwroot/assets")
        {
        }

        public class ConsolableHandler : IConsolableHandler
        {
            private readonly MiruSolution _solution;

            public ConsolableHandler(MiruSolution solution) => 
                _solution = solution;
            
            public Task Execute()
            {
                var wwwrootStorage = _solution.AppDir / "wwwroot" / "assets";
                var storageApp = _solution.RootDir / "storage" / "assets";

                Console2.GreenLine($"Creating symlink");
                Console2.WhiteLine($"\tfrom: {storageApp}");
                Console2.WhiteLine($"\tto:   {wwwrootStorage}");
                
                Directories.CreateIfNotExists(storageApp);

                if (Directory.Exists(wwwrootStorage))
                {
                    Console2.YellowLine($"Directory already exists... Skiping");
                }
                else
                {
                    Directory.CreateSymbolicLink(wwwrootStorage, storageApp);
                    Console2.GreenLine("Done");
                }
                
                return Task.CompletedTask;
            }
        }
    }
}