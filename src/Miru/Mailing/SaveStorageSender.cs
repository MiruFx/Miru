using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using FluentEmail.Core;
using FluentEmail.Core.Interfaces;
using FluentEmail.Core.Models;
using Microsoft.Extensions.Logging;
using Miru.Core;
using Miru.Foundation;
using Miru.Storages;

namespace Miru.Mailing
{
    public class SaveStorageSender : ISender
    {
        private readonly Storage _storage;
        private readonly ILogger<ISender> _logger;

        public SaveStorageSender(Storage storage, ILogger<ISender> logger)
        {
            _storage = storage;
            _logger = logger;
        }

        public SendResponse Send(IFluentEmail email, CancellationToken? token = null)
        {
            return SendAsync(email, token).GetAwaiter().GetResult();
        }

        public async Task<SendResponse> SendAsync(IFluentEmail email, CancellationToken? token = null)
        {
            var response = new SendResponse();
            await SaveEmailToDiskAsync(email);
            return response;
        }

        private async Task SaveEmailToDiskAsync(IFluentEmail email)
        {
            var emailFile = _storage.Temp() / "emails" / $"{email.Data.Subject}-{DateTime.Now:yyyy-MM-dd_HH-mm-ss.fff}.txt";
            
            Directories.CreateForPathIfNotExists(emailFile);
            
            await File.WriteAllTextAsync(emailFile, email.ToRawEmail());

            _logger.LogDebug($"Email {email.Data.Subject} saved as {emailFile}");
        }
    }
}