namespace MiruNext.Framework;

public static class MiruRegistry
{
    public static IServiceCollection AddMiruNext<TProgram, THtmlConfig>(
        this IServiceCollection services)
            where THtmlConfig : HtmlConventions, new()
    {
        services
            .AddFastEndpoints()
            .AddAuthorization()
            .AddRazorViews<TProgram>()
            
            .AddHtmlConventions<THtmlConfig>();
            
        return services;
    }
}