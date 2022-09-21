using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using FluentFTP;
using Miru.Core;
using Miru.Storages;
using Miru.Storages.Ftp;

namespace Miru.Storage.Ftp;

public class FtpStorage : IStorage
{
    public FtpClient Client { get; }

    public virtual MiruPath StorageDir => A.Path;

    public virtual MiruPath App => StorageDir / "app";

    public MiruPath Assets => throw new NotImplementedException("FtpStorage does not support Assets");
    
    public FtpStorage(FtpOptions ftpOptions)
    {
        Client = new FtpClient(ftpOptions.FtpServer)
        {
            Credentials = new NetworkCredential(ftpOptions.FtpUser, ftpOptions.FtpPassword),
            Port = ftpOptions.FtpPort,
        };
    }

    public async Task PutAsync(MiruPath remotePath, MiruPath sourcePath)
    {
        await EnsureClientIsConnectedAsync();
            
        var path = App / remotePath;
        
        Miru.App.Log.Debug(
            "FtpStorage is saving from source path {SourcePath} to {RemotePath}",
            sourcePath,
            path);
        
        await Client.UploadFileAsync(sourcePath, App / remotePath, FtpRemoteExists.Overwrite, createRemoteDir: true);
    }

    public async Task PutAsync(MiruPath remotePath, Stream stream)
    {
        await EnsureClientIsConnectedAsync();

        var path = App / remotePath;
        
        Miru.App.Log.Debug(
            "FtpStorage is saving stream sized {StreamSize} into {RemotePath}",
            stream.Length,
            path);
            
        await Client.UploadAsync(stream, path, FtpRemoteExists.Overwrite, createRemoteDir: true);
    }

    public async Task<Stream> GetAsync(MiruPath remotePath)
    {
        await EnsureClientIsConnectedAsync();
            
        Miru.App.Log.Debug(
            "FtpStorage is getting stream from {RemotePath}",
            remotePath);
        
        var stream = new MemoryStream();
            
        await Client.DownloadAsync(stream, App / remotePath);
            
        return stream;
    }

    public async Task<bool> FileExistsAsync(MiruPath remote)
    {
        await EnsureClientIsConnectedAsync();
            
        return await Client.FileExistsAsync(App / remote);
    }

    private async Task EnsureClientIsConnectedAsync()
    {
        if (Client.IsConnected == false) 
            await Client.ConnectAsync();
    }
}