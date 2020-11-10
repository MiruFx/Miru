using System;
using AutoFixture;
using Bogus;
using Microsoft.Extensions.DependencyInjection;
using Miru.Fabrication.FixtureConventions;

namespace Miru.Fabrication
{
    public static class FabricationServiceCollectionExtensions
    {
        public static IServiceCollection AddFabrication<TFabricator>(
            this IServiceCollection services,
            Action<ConventionExpression> conventions = null) 
            where TFabricator : Fabricator
        {
            services.Scan(scan => scan
                .FromAssemblies(typeof(TFabricator).Assembly)
                .AddClasses(classes => classes.AssignableTo(typeof(ICustomFabricator<>)))
                    .AsImplementedInterfaces()
                    .As<ICustomFabricator>()
                    .WithSingletonLifetime());
            
            services.AddSingleton<Faker>();
            
            services.AddSingleton<FabricatedSession, FabricatedSession>();
            
            services.AddSingleton(sp =>
            {
                var fixture = new Fixture();
                var faker = sp.GetService<Faker>();
                var session = sp.GetService<FabricatedSession>();

                fixture.AddDefaultMiruConvention(faker);
                
                if (conventions != null) 
                    fixture.AddConvention(faker, conventions);
                
                // TODO: better name for this builder
                fixture.Customizations.Add(new FabricationSpecimenBuilder(fixture, session));

                return fixture;
            });

            services.AddSingleton<FabSupport>();
            
            services.AddSingleton<TFabricator>();

            services.AddSingleton<Fabricator>(sp => sp.GetRequiredService<TFabricator>());

            return services;
        }
    }
}