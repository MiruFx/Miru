using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Miru.Core;

namespace Miru.Storages;

public static class StorageExtensions
{
    public static async Task PutBase64Async(this IStorage storage, MiruPath remotePath, string base64)
    {
        var data = System.Convert.FromBase64String(base64);

        await using var stream = new MemoryStream(data);
            
        await storage.PutAsync(remotePath, stream);
    }
        
    public static async Task<string> GetBase64Async(this IStorage storage, MiruPath remotePath)
    {
        await using var stream = await storage.GetAsync(remotePath);
            
        return System.Convert.ToBase64String(stream.ToBytes());
    }
        
    public static async Task PutContentAsync(this IStorage storage, MiruPath remotePath, string content)
    {
        await using var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
            
        await storage.PutAsync(remotePath, stream);
    }
    
    public static async Task<SaveResult> PutFormAsync(
        this IStorage storage, 
        MiruPath fullPath, 
        IFormFile formFile)
    {
        await using var stream = formFile.OpenReadStream();
        
        await storage.PutAsync(fullPath, stream);
        
        return new SaveResult
        {
            FizeSize = (storage.Path / fullPath).FileSize(),
            RelativePath = fullPath
        };
    }
    
    public static async Task DeleteAsync(this IStorage storage, MiruPath remotePath)
    {
        var fullRemotePath = storage.Path / remotePath;
            
        File.Delete(fullRemotePath);

        await Task.CompletedTask;
    }
    
    public static MiruPath Temp(this IAppStorage storage)
    {
        return storage.Root / "temp";
    }
    
    public static bool FileExists(this IStorage storage, MiruPath path) =>
        storage.FileExistsAsync(path).GetAwaiter().GetResult();
    
    public static MiruPath RelativePath(this IStorage storage, MiruPath fullPath) => 
        Path.GetRelativePath(storage.Path, fullPath);
}

public class SaveResult
{
    public long FizeSize { get; set; }
    public string RelativePath { get; set; }
}