namespace MiruNext.Framework;

public static class MiruRegistry
{
    public static IServiceCollection AddMiruNext<THtmlConfig>(
        this IServiceCollection services)
            where THtmlConfig : HtmlConventions, new()
    {
        services
            .AddFastEndpoints()
            .AddAuthorization()
            
            .AddRazorComponents().Services
            .AddHttpContextAccessor()
            .AddDistributedMemoryCache()
            .AddAntiforgery()
            // .AddTransient<SessionManager>()
            .AddHttpContextAccessor()
            .AddResponseCompression(options =>
            {
                options.EnableForHttps = true;
            })
            .AddSession(options => {
                options.Cookie.Name = ".sanus-plus";
                options.IdleTimeout = TimeSpan.FromMinutes(1);
            })
            .AddHtmlConventions<THtmlConfig>();
            
        return services;
    }
}