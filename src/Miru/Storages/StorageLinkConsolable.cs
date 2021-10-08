using System.Threading.Tasks;
using Emet.FileSystems;
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

                Directories.CreateIfNotExists(storageApp);
            
                FileSystem.CreateSymbolicLink(
                    storageApp,
                    wwwrootStorage,
                    FileType.Directory);

                Console2.GreenLine($"Created a symlink");
                Console2.WhiteLine($"\tfrom: {storageApp}");
                Console2.WhiteLine($"\tto:   {wwwrootStorage}");

                return Task.CompletedTask;
            }
        }
    }
}