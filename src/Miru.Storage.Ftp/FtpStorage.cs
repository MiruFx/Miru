using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using FluentFTP;
using Miru.Core;

namespace Miru.Storages.Ftp
{
    public class FtpStorage : IStorage
    {
        private readonly FtpClient _client;

        public FtpStorage(FtpOptions ftpOptions)
        {
            _client = new FtpClient(ftpOptions.FtpServer)
            {
                Credentials = new NetworkCredential(ftpOptions.FtpUser, ftpOptions.FtpPassword),
                Port = ftpOptions.FtpPort,
            };
        }

        public MiruPath StorageDir => A.Path;

        public MiruPath App => StorageDir / "app";

        public MiruPath Assets => throw new NotImplementedException("Ftp storage does not support Assets");

        public async Task PutAsync(MiruPath remotePath, MiruPath sourcePath)
        {
            await EnsureClientIsConnectedAsync();
            
            await _client.UploadFileAsync(sourcePath, App / remotePath, FtpRemoteExists.Overwrite, createRemoteDir: true);
        }

        public async Task PutAsync(MiruPath remotePath, Stream stream)
        {
            await EnsureClientIsConnectedAsync();

            Miru.App.Log.Information($"FtpStorage: Saving stream into {App / remotePath}");
            
            await _client.UploadAsync(stream, App / remotePath, FtpRemoteExists.Overwrite, createRemoteDir: true);
        }

        public async Task<Stream> GetAsync(MiruPath remotePath)
        {
            await EnsureClientIsConnectedAsync();
            
            Miru.App.Log.Information($"FtpStorage: Reading stream from {App / remotePath}");

            var stream = new MemoryStream();
            
            await _client.DownloadAsync(stream, App / remotePath);
            
            return stream;
        }

        public async Task<bool> FileExistsAsync(MiruPath remote)
        {
            await EnsureClientIsConnectedAsync();
            
            return await _client.FileExistsAsync(App / remote);
        }

        private async Task EnsureClientIsConnectedAsync()
        {
            if (_client.IsConnected == false) 
                await _client.ConnectAsync();
        }
    }
}