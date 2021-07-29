using System.IO;
using System.Threading.Tasks;
using Miru.Core;

namespace Miru.Storages
{
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
        
        public static MiruPath Temp(this IStorage storage)
        {
            return storage.StorageDir / "temp";
        }
    }
}