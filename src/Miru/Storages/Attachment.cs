using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using Microsoft.AspNetCore.Http;
using Miru.Behaviors.TimeStamp;
using Miru.Domain;

namespace Miru.Storages;

[Table("Attachments")]
public class Attachment :
    Entity, 
    ITimeStamped
{
    public string Key { get; set; }
    public string FileName { get; set; }
    public string ContentType { get; set; }
    public long ByteSize { get; set; }
    public string Checksum { get; set; }
    
    public string Entity { get; set; }
    public string Property { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    [NotMapped]
    public MiruPath FullPath { get; set; }

    public Attachment()
    {
    }

    public Attachment(IStorage storage, MiruPath filePath)
    {
        Key = filePath.RelativeTo(storage);
        FileName = filePath.FileName();
        ByteSize = filePath.FileSize();
        ContentType = filePath.ContentType();
        Checksum = filePath.GetMd5();
    }

    public Attachment(MiruPath filePath, Stream stream, IFormFile formFile, MiruPath fullPath)
    {
        // In LocalDisk storage it is the path in the storage
        // Key is the value that point where the file is
        Key = filePath;
        FileName = Path.GetFileName(filePath);
        ByteSize = stream.Length;
        ContentType = formFile.ContentType;
        Checksum = fullPath.GetMd5();
        FullPath = fullPath;
    }
    
    public void UpdateFileMetadata(Attachment attachment)
    {
        ByteSize = attachment.ByteSize;
        ContentType = attachment.ContentType;
        Checksum = attachment.Checksum;
        UpdatedAt = App.Now();
    }
}