using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Miru.Core;

namespace Miru.Storages;

public interface IStorage
{
    MiruPath StorageDir { get; }
        
    // TODO: Should be removed and create a AssetsStorage and AppStorage
    MiruPath App { get; }
    MiruPath Assets { get; }
        
    Task PutAsync(MiruPath remote, MiruPath source);
        
    Task PutAsync(MiruPath remote, Stream stream);
        
    Task<Stream> GetAsync(MiruPath remote);
        
    Task<bool> FileExistsAsync(MiruPath remote);

    Task<List<MiruPath>> GetFilesAsync(MiruPath path, CancellationToken ct = default);
}