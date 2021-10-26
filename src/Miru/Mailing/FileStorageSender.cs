using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentEmail.Core;
using FluentEmail.Core.Interfaces;
using FluentEmail.Core.Models;
using Microsoft.Extensions.Logging;
using Miru.Core;
using Miru.Storages;

namespace Miru.Mailing;

public class FileStorageSender : ISender
{
    private readonly IStorage _storage;
    private readonly ILogger<ISender> _logger;

    public FileStorageSender(IStorage storage, ILogger<ISender> logger)
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
        await SaveEmailToDisk(email);
        return response;
    }

    private async Task SaveEmailToDisk(IFluentEmail email)
    {
        var emailFile = _storage.Temp() / "emails" / $"{email.Data.Subject}-{DateTime.Now:yyyy-MM-dd_HH-mm-ss.fff}.txt";
            
        emailFile.Dir().EnsureDirExist();

        await File.WriteAllTextAsync(emailFile, email.ToRawEmail());

        var desinations = email.Data.ToAddresses.Select(x => x.EmailAddress).Join(",");
        
        _logger.LogDebug($"Email '{email.Data.Subject}' to '{desinations}' saved at {emailFile}");
    }
}