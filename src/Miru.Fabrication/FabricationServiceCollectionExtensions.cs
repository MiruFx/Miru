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
            services.AddFabrication(conventions);
            
            services.AddSingleton<TFabricator>();
            services.ReplaceSingleton<Fabricator>(sp => sp.GetRequiredService<TFabricator>());
            
            return services;
        }
        
        public static IServiceCollection AddFabrication(
            this IServiceCollection services,
            Action<ConventionExpression> conventions = null)
        {
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
            
            services.AddSingleton<Fabricator>();

            return services;
        }
    }
}