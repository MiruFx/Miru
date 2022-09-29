using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FluentFTP;
using Miru.Core;
using Miru.Storages;

namespace Miru.Storage.Ftp;

public class FtpStorage : IStorage
{
    public FtpClient Client { get; }

    public MiruPath Path => Root;
    
    public MiruPath Root { get; protected set; }

    public FtpStorage(FtpOptions ftpOptions)
    {
        Client = new FtpClient(ftpOptions.Host)
        {
            Credentials = new NetworkCredential(ftpOptions.User, ftpOptions.Password),
            Port = ftpOptions.Port,
        };
        
        Root = ftpOptions.Root;
    }

    public async Task PutAsync(MiruPath remotePath, MiruPath sourcePath)
    {
        await EnsureClientIsConnectedAsync();
            
        var path = Path / remotePath;
        
        Miru.App.Log.Debug(
            "FtpStorage is saving from source path {SourcePath} to {RemotePath}",
            sourcePath,
            path);
        
        Client.UploadFile(sourcePath, Path / remotePath, FtpRemoteExists.Overwrite, createRemoteDir: true);

        await Task.CompletedTask;
    }

    public async Task PutAsync(MiruPath remotePath, Stream stream)
    {
        await EnsureClientIsConnectedAsync();

        var path = Path / remotePath;
        
        App.Log.Debug(
            "FtpStorage is saving stream sized {StreamSize} into {RemotePath}",
            stream.Length,
            path);
            
        Client.UploadStream(stream, path, FtpRemoteExists.Overwrite, createRemoteDir: true);
        
        await Task.CompletedTask;
    }

    public async Task<Stream> GetAsync(MiruPath remotePath)
    {
        await EnsureClientIsConnectedAsync();
            
        App.Log.Debug(
            "FtpStorage is getting stream from {RemotePath}",
            remotePath);
        
        var stream = new MemoryStream();
            
        Client.DownloadStream(stream, Path / remotePath);
            
        return stream;
    }

    public async Task<bool> FileExistsAsync(MiruPath remote)
    {
        await EnsureClientIsConnectedAsync();
            
        return Client.FileExists(Path / remote);
    }

    public async Task<List<MiruPath>> GetFilesAsync(MiruPath path, CancellationToken ct = default)
    {
        await EnsureClientIsConnectedAsync();
        
        var files = Client.GetNameListing(path);

        return files.Select(x => new MiruPath(x)).ToList();
    }
    
    private async Task EnsureClientIsConnectedAsync()
    {
        if (Client.IsConnected == false) 
            Client.Connect();

        await Task.CompletedTask;
    }
}