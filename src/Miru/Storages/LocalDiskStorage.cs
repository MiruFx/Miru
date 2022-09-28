using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Miru.Core;

namespace Miru.Storages;

public class LocalDiskStorage : IStorage
{
    public LocalDiskStorage(MiruSolution solution)
    {
        _solution = solution;
            
        StorageDir = _solution.StorageDir;
    }
        
    private readonly MiruSolution _solution;

    public MiruPath StorageDir { get; protected set; }

    public virtual MiruPath App => StorageDir / "app"; 
        
    public virtual MiruPath Assets => _solution.StorageDir / "assets"; 
        
    public async Task PutAsync(MiruPath remote, MiruPath source)
    {
        var fullRemoteDir = App / remote;
            
        fullRemoteDir.Dir().EnsureDirExist();
            
        File.Copy(source, fullRemoteDir, true);

        await Task.CompletedTask;
    }

    public async Task PutAsync(MiruPath remote, Stream stream)
    {
        var fullRemoteDir = App / remote;
            
        Files.DeleteIfExists(fullRemoteDir);
            
        fullRemoteDir.Dir().EnsureDirExist();

        await using var fileStream = File.Create(fullRemoteDir);
            
        stream.Seek(0, SeekOrigin.Begin);
            
        await stream.CopyToAsync(fileStream);
            
        fileStream.Close();
    }

    public async Task<Stream> GetAsync(MiruPath remote)
    {
        return await Task.FromResult(File.OpenRead(App / remote));
    }

    public async Task<bool> FileExistsAsync(MiruPath remote)
    {
        return await Task.FromResult(File.Exists(App / remote));
    }

    public async Task<List<MiruPath>> GetFilesAsync(MiruPath path, CancellationToken ct = default)
    {
        return await Task.FromResult(Directory
            .GetFiles(path, "*.*")
            .Select(x => new MiruPath(x))
            .ToList());
    }
}