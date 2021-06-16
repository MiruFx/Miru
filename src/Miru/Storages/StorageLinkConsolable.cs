using Emet.FileSystems;
using Miru.Consolables;
using Miru.Core;
using Oakton;

namespace Miru.Storages
{
    [Description("Create a symlink from /storage/assets to {{App}}/wwwroot/assets", Name = "storage:link")]
    public class StorageLinkConsolable : ConsolableSync
    {
        private readonly MiruSolution _solution;
    
        public StorageLinkConsolable(MiruSolution solution)
        {
            _solution = solution;
        }
    
        public override void Execute()
        {
            var wwwrootStorage = _solution.AppDir / "wwwroot" / "storage";
            var storageApp = _solution.StorageDir / "storage" / "assets";
            
            Directories.CreateIfNotExists(storageApp);
            
            FileSystem.CreateSymbolicLink(
                storageApp,
                wwwrootStorage,
                FileType.Directory);

            Console2.GreenLine($"Created a symlink");
            Console2.WhiteLine($"\tfrom: {storageApp}");
            Console2.WhiteLine($"\tto:   {wwwrootStorage}");
        }
    }
}