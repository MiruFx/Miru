using System;
using System.Linq;
using Baseline;
using AreaFeatureFolderOptions = Miru.Mvc.FeaturesFolder.AreaFeatureFolderOptions;
using FeatureControllerModelConvention = Miru.Mvc.FeaturesFolder.FeatureControllerModelConvention;
using FeatureFolderOptions = Miru.Mvc.FeaturesFolder.FeatureFolderOptions;
using FeatureViewLocationExpander = Miru.Mvc.FeaturesFolder.FeatureViewLocationExpander;

// ReSharper disable once CheckNamespace

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        ///     Use feature folders with custom options
        /// </summary>
        public static IMvcCoreBuilder AddFeatureFolders(this IMvcCoreBuilder services, FeatureFolderOptions options)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            if (options == null)
                throw new ArgumentException(nameof(options));

            var expander = new FeatureViewLocationExpander(options);

            services.AddMvcOptions(o => o.Conventions.Add(new FeatureControllerModelConvention(options)))
                .AddRazorViewEngine(o =>
                {
                    // o.ViewLocationFormats.Clear();
                    o.ViewLocationFormats.Add(options.FeatureNamePlaceholder + @"\{0}.cshtml");
                    o.ViewLocationFormats.Add(options.FeatureFolderName + @"\Shared\{0}.cshtml");
                    o.ViewLocationFormats.Add(options.DefaultViewLocation);

                    o.ViewLocationExpanders.Add(expander);
                });

            return services;
        }

        /// <summary>
        ///     Use areas with feature folders and custom options
        /// </summary>
        public static IMvcCoreBuilder AddAreaFeatureFolders(this IMvcCoreBuilder services, AreaFeatureFolderOptions options)
        {
            services.AddRazorViewEngine(o =>
            {
                var current = o.AreaViewLocationFormats.ToList();
                
                o.AreaViewLocationFormats.Clear();
                
                o.AreaViewLocationFormats.Add(options.DefaultAreaViewLocation);
                o.AreaViewLocationFormats.Add(options.AreaFolderName + @"\{2}\{1}\{0}.cshtml");
                o.AreaViewLocationFormats.Add(options.AreaFolderName + @"\{2}\Shared\{0}.cshtml");
                o.AreaViewLocationFormats.Add(options.AreaFolderName + @"\Shared\{0}.cshtml");
                o.AreaViewLocationFormats.Add(options.FeatureFolderName + @"\Shared\{0}.cshtml");

                o.AreaViewLocationFormats.AddRange(current);
            });

            return services;
        }

        /// <summary>
        ///     Use feature folders with the default options. Controllers and view will be located
        ///     under a folder named Features. Shared views are located in Features\Shared.
        /// </summary>
        public static IMvcCoreBuilder AddFeatureFolders(this IMvcCoreBuilder services)
        {
            return AddFeatureFolders(services, new FeatureFolderOptions());
        }

        /// <summary>
        ///     Use areas with feature folders with the default options. Controllers and views will
        ///     be located under a folder named Areas with an area specific folder. Shared views are
        ///     located in Areas\Shared and then in Features\Shared 
        /// </summary>
        public static IMvcCoreBuilder AddAreaFeatureFolders(this IMvcCoreBuilder services)
        {
            return AddAreaFeatureFolders(services, new AreaFeatureFolderOptions());
        }
    }
}