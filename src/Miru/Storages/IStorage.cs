using System.IO;
using System.Threading.Tasks;
using Miru.Core;

namespace Miru.Storages;

public interface IStorage
{
    MiruPath StorageDir { get; }
        
    MiruPath App { get; }
        
    MiruPath Assets { get; }
        
    Task PutAsync(MiruPath remote, MiruPath source);
        
    Task PutAsync(MiruPath remote, Stream stream);
        
    Task<Stream> GetAsync(MiruPath remote);
        
    Task<bool> FileExistsAsync(MiruPath remote);
}