using System;
using System.Linq;
using HtmlTags.Conventions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Miru.Html;
using Miru.Mvc;

namespace Miru
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMiruMvc(
            this IServiceCollection services, 
            Action<MvcOptions> mvc = null,
            Action<MiruMvcOptions> setupAction = null)
        {
            var miruOptions = new MiruMvcOptions();
            
            setupAction?.Invoke(miruOptions);

            services.Configure<RazorViewEngineOptions>(o =>
            {
                // {2} is area, {1} is controller,{0} is the action
                // the component's path "Components/{ViewComponentName}/{ViewComponentViewName}" is in the action {0}
                o.ViewLocationFormats.Add("/{0}" + RazorViewEngine.ViewExtension);        
            });

            var mvcCoreBuilder = services.AddMvcCore(options =>
                {
                    options.Filters.Add(typeof(AutoValidateAntiforgeryTokenAttribute));
                    options.Filters.Add(typeof(ExceptionFilter));
                    options.Filters.Add(typeof(ViewDataFilter));

                    var index = options.ValueProviderFactories
                        .IndexOf(options.ValueProviderFactories.OfType<QueryStringValueProviderFactory>().Single());
                    
                    options.ValueProviderFactories[index] = new CulturedQueryStringValueProviderFactory();
                    
                    mvc?.Invoke(options);
                })
                .AddMiruActionResult()
                // .AddRazorRuntimeCompilation()
                .AddMiruNestedControllers();

            if (miruOptions.EnableFeatureFolders)
            {
                mvcCoreBuilder.AddFeatureFolders();
                mvcCoreBuilder.AddAreaFeatureFolders();
            }

            services.AddAntiforgery(options =>
            {
                options.HeaderName = "X-CSRF-Token";
            });
            
            services.Configure<RouteOptions>(options =>
            {
                // options.LowercaseUrls = true;
                // options.LowercaseQueryStrings = true;
            });
            
            services.AddHttpContextAccessor();

            services.AddSingleton<AssetsMap>();

            services.AddSingleton<IActionResultExecutor<HtmlTagResult>, ContentResultExecutor>();
                
            return services;
        }

        public static IServiceCollection ForwardScoped<TFrom, TTo>(this IServiceCollection services) 
            where TTo : TFrom 
            where TFrom : class
        {
            return services.AddScoped<TFrom>(x => x.GetRequiredService<TTo>());
        }
        
        public static IServiceCollection ForwardSingleton<TFrom, TTo>(this IServiceCollection services) 
            where TTo : TFrom 
            where TFrom : class
        {
            return services.AddSingleton<TFrom>(x => x.GetRequiredService<TTo>());
        }
        
        public static IServiceCollection ReplaceTransient<TService, TImplementation>(this IServiceCollection services)
        {
            return services.Replace(
                new ServiceDescriptor(typeof(TService), typeof(TImplementation), ServiceLifetime.Transient));
        }
        
        public static IServiceCollection ReplaceSingleton<TService>(
            this IServiceCollection services, 
            Func<IServiceProvider, object> implementationFactory)
        {
            return services.Replace(
                new ServiceDescriptor(typeof(TService), implementationFactory, ServiceLifetime.Singleton));
        }
        
        public static IServiceCollection ReplaceSingleton<TService>(
            this IServiceCollection services, 
            TService service)
        {
            return services.Replace(
                new ServiceDescriptor(typeof(TService), service));
        }
        
        public static IServiceCollection ReplaceSingleton<TService, TImplementation>(this IServiceCollection services)
        {
            return services.Replace(
                new ServiceDescriptor(typeof(TService), typeof(TImplementation), ServiceLifetime.Singleton));
        }
        
        public static IServiceCollection ReplaceScoped<TService>(this IServiceCollection services)
        {
            return services.Replace(
                new ServiceDescriptor(typeof(TService), typeof(TService), ServiceLifetime.Scoped));
        }
        
        public static IServiceCollection ReplaceScoped<TService, TImplementation>(this IServiceCollection services)
        {
            return services.Replace(
                new ServiceDescriptor(typeof(TService), typeof(TImplementation), ServiceLifetime.Scoped));
        }
        
        public static IServiceCollection AddBothSingleton<TService, TImplementation>(this IServiceCollection services) 
            where TService : class 
            where TImplementation : class, TService
        {
            services.AddSingleton<TImplementation>();

            services.AddSingleton<TService>(sp => sp.GetService<TImplementation>());

            return services;
        }
        
        public static IServiceCollection AddServiceCollection(this IServiceCollection services)
        {
            services.AddSingleton(services);
            return services;
        }
    }
}
