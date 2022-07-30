using Microsoft.Extensions.Hosting;

namespace Miru.Hosting;

public static class HostingEnvironmentExtensions
{
    public static bool IsTest(this IHostEnvironment env) => 
        env.IsEnvironment("Test");
        
    public static bool IsDevelopmentOrTest(this IHostEnvironment env) => 
        env.IsDevelopment() || env.IsTest();
        
    public static bool IsNotDevelopmentOrTest(this IHostEnvironment env) => 
        env.IsDevelopmentOrTest() == false;
    
    public static bool IsNotTest(this IHostEnvironment environment) => 
        environment.IsTest() == false;
}