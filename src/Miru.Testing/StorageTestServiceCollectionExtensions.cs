using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Miru.Consolables;
using Miru.Core;
using Miru.Storages;

namespace Miru.Testing
{
    public static class StorageTestServiceCollectionExtensions
    {
        public static IServiceCollection AddTestStorage(this IServiceCollection services)
        {
            return services.AddStorage<TestStorage>();
        }
        
        public static FormFile FormFile(this ITestFixture fixture, MiruPath path, string contentType)
        {
            var stream = File.OpenRead(path);
            
            var file = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(path))
            {
                Headers = new HeaderDictionary(),
                ContentType = contentType
            };

            return file;
        }
    }
}