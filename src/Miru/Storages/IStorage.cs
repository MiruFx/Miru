using Miru.Core;

namespace Miru.Storages
{
    public interface IStorage
    {
        MiruPath StorageDir { get; }
        
        MiruPath App { get; }
        
        MiruPath Assets { get; }
    }
}