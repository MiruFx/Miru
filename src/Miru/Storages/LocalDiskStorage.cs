using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Miru.Core;

namespace Miru.Storages;

public abstract class LocalDiskStorage : IStorage
{
    public MiruPath Root { get; protected set; }

    public MiruPath StorageDir { get; }

    public MiruPath Path => Root;

    public LocalDiskStorage(MiruSolution solution)
    {
        StorageDir = solution.StorageDir;
        Root = StorageDir;
    }
    
    public async Task PutAsync(MiruPath remote, MiruPath source)
    {
        var fullRemoteDir = Path / remote;
            
        fullRemoteDir.Dir().EnsureDirExist();
            
        File.Copy(source, fullRemoteDir, true);

        await Task.CompletedTask;
    }

    public async Task PutAsync(MiruPath remote, Stream stream)
    {
        var fullRemoteDir = Path / remote;
            
        Files.DeleteIfExists(fullRemoteDir);
            
        fullRemoteDir.Dir().EnsureDirExist();

        await using var fileStream = File.Create(fullRemoteDir);
            
        stream.Seek(0, SeekOrigin.Begin);
            
        await stream.CopyToAsync(fileStream);
            
        fileStream.Close();
    }

    public async Task<Stream> GetAsync(MiruPath remote)
    {
        return await Task.FromResult(File.OpenRead(Path / remote));
    }

    public async Task<bool> FileExistsAsync(MiruPath remote)
    {
        return await Task.FromResult(File.Exists(Path / remote));
    }

    public async Task<List<MiruPath>> GetFilesAsync(MiruPath path, CancellationToken ct = default)
    {
        return await Task.FromResult(Directory
            .GetFiles(path, "*.*")
            .Select(x => new MiruPath(x))
            .ToList());
    }
}