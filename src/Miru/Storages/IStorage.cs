using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Miru.Core;

namespace Miru.Storages;

public interface IStorage
{
    MiruPath Path { get; }

    MiruPath Root { get; }
        
    Task PutAsync(MiruPath remote, MiruPath source);
        
    Task PutAsync(MiruPath remote, Stream stream);
        
    Task<Stream> GetAsync(MiruPath remote);
        
    Task<bool> FileExistsAsync(MiruPath remote);

    Task<List<MiruPath>> ListFilesAsync(MiruPath path, CancellationToken ct = default);
}