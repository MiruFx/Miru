using System.IO;
using System.Security.Cryptography;
using FluentMigrator.Builders.Alter.Table;
using FluentMigrator.Builders.Create.Table;
using Microsoft.AspNetCore.Http;
using MimeTypes;
using Miru.Databases.Migrations.FluentMigrator;

namespace Miru.Storages;

public static class Storage2Extensions
{
    public static async Task WriteAllTextAsync(
        this MiruPath path,
        string content)
    {
        path.Dir().EnsureDirExist();
            
        await File.WriteAllTextAsync(path, content);
    }
    
    public static async Task GetAsync(
        this IStorage sourceStorage,
        MiruPath sourcePath, 
        IStorage destinationStorage,
        MiruPath destinationPath)
    {
        await using var remoteFileStream = await sourceStorage.GetAsync(sourcePath);
        await destinationStorage.PutAsync(destinationPath, remoteFileStream);
    }
    
    public static async Task MoveDiskFileAsync(
        this IStorage storage,
        MiruPath from, 
        MiruPath to,
        bool overwrite = false,
        CancellationToken ct = default)
    {
        to.Dir().EnsureDirExist();
        
        if (overwrite)
            to.DeleteFileIfExists();
        
        File.Move(from, to);

        await Task.CompletedTask;
    }
    
    // public async static Task<List<MiruPath>> ListDiskFilesAsync(
    //     this IStorage storage,
    //     MiruPath path, 
    //     string pattern = "*.*",
    //     CancellationToken ct = default)
    // {
    //     return await Task.FromResult(Directory
    //         .GetFiles(storage.Path / path, pattern)
    //         .Select(x => new MiruPath(x))
    //         .ToList());
    // }
    
    public static string ContentType(this MiruPath miruPath) => 
        MimeTypeMap.GetMimeType(miruPath);
    
    public static async Task PurgeAsync(this IAppStorage storage, Attachment attachment)
    {
        var remotePath = attachment.Key;
        
        var fullRemotePath = storage.Path / remotePath;
            
        File.Delete(fullRemotePath);

        await Task.CompletedTask;
    }
    
    // public async static Task AttachAsync<TEntity, TProperty>(
    //     this IAppStorage storage, 
    //     MiruPath filePath,
    //     IFormFile formFile,
    //     TEntity entity,
    //     Expression<Func<TEntity, TProperty>> property) 
    //         where TEntity : IEntity 
    //         where TProperty : Attachment
    // {
    //     await using var stream = formFile.OpenReadStream();
    //
    //     var fullPath = storage.Path / filePath;
    //     
    //     await storage.PutAsync(filePath, stream);
    //
    //     var accessor = Baseline.Reflection.ReflectionHelper.GetAccessor(property);
    //
    //     // TODO: use constructor
    //     var attachment = new Attachment 
    //     {
    //         // In LocalDisk storage it is the path in the storage
    //         // Key is the value that point where the file is
    //         Key = filePath, 
    //         FileName = Path.GetFileName(filePath), 
    //         Entity = typeof(TEntity).Name, 
    //         ByteSize = stream.Length,
    //         ContentType = formFile.ContentType,
    //         Checksum = fullPath.GetMd5(),
    //         Property = accessor.Name
    //     };
    //
    //     accessor.SetValue(entity, attachment);
    // }
    
    public static Attachment Attach2(this IAppStorage storage, MiruPath fullPath) =>
        new Attachment
        {
            Key = fullPath.RelativeTo(storage), 
            FileName = fullPath.FileName(), 
            ByteSize = fullPath.FileSize(),
            ContentType = fullPath.ContentType(),
            Checksum = fullPath.GetMd5(),
        };

    public static async Task<Attachment> AttachAsync2(
        this IAppStorage storage,
        MiruPath filePath,
        IFormFile formFile)
    {
        // TODO: should throw exception
        if (formFile is null)
            return null;
        
        var fullPath = storage.Path / filePath;
        var relativePath = storage.RelativePath(fullPath);
        
        using (var stream = formFile.OpenReadStream())
        {
            await storage.PutAsync(filePath, stream);

            return new Attachment(relativePath, stream, formFile, fullPath);
        }
    }
    
    public async static Task<Attachment> AttachBase64Async(
        this IAppStorage storage, 
        MiruPath remotePath,
        string base64)
    {
        var data = Convert.FromBase64String(base64);

        await using var stream = new MemoryStream(data);
            
        await storage.PutAsync(remotePath, stream);

        return new Attachment 
        {
            Key = remotePath, 
            FileName = remotePath.FileName(), 
            ContentType = remotePath.ContentType(),
            ByteSize = stream.Length,
            Checksum = stream.GetMd5(),
        };
    }
    
    public static MiruPath PathFor(this IStorage storage, MiruPath forPath) =>
        storage.Path / forPath;

    public static string FileExtensionWithDot(this IFormFile formFile) => 
        Path.GetExtension(formFile.FileName);

    public static string GetMd5(this MiruPath path)
    {
        using var md5 = MD5.Create();
        
        using var stream = File.OpenRead(path);
        
        var hash = md5.ComputeHash(stream);
        
        return BitConverter.ToString(hash).Replace("-", string.Empty).ToLowerInvariant();
    }
    
    public static string GetMd5(this Stream stream)
    {
        using var md5 = MD5.Create();
        
        var hash = md5.ComputeHash(stream);
        
        return BitConverter.ToString(hash).Replace("-", string.Empty).ToLowerInvariant();
    }

    public static MiruPath FullPath(this IAppStorage storage, MiruPath relativePath) =>
        storage.Path / relativePath;
    
    public static ICreateTableColumnOptionOrForeignKeyCascadeOrWithColumnSyntax AsStorageAttachment(
        this ICreateTableColumnAsTypeSyntax column, 
        string attachmentsTable = "Attachments",
        bool deleteOnCascade = true)
    {
        return column.AsForeignKeyReference(attachmentsTable, deleteOnCascade: deleteOnCascade);
    }
    
    public static IAlterTableColumnOptionOrAddColumnOrAlterColumnSyntax AsStorageAttachment(
        this IAlterTableColumnAsTypeSyntax column, 
        string attachmentsTable = "Attachments",
        bool deleteOnCascade = true)
    {
        return column.AsForeignKeyReference(attachmentsTable, deleteOnCascade: deleteOnCascade);
    }
}