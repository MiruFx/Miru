using Microsoft.Extensions.DependencyInjection;
using Miru.Foundation;

namespace Miru.Storages
{
    public static class StorageServiceCollectionExtensions
    {
        public static IServiceCollection AddStorage(this IServiceCollection services)
        {
            return services.AddSingleton<Storage>();
        }
    }
}