using Microsoft.Extensions.Hosting;

namespace Miru.Foundation.Hosting
{
    public static class HostingEnvironmentExtensions
    {
        public static bool IsTest(this IHostEnvironment env) => env.IsEnvironment("Test");
        
        public static bool IsDevelopmentOrTest(this IHostEnvironment env) => env.IsDevelopment() || env.IsTest();
    }
}