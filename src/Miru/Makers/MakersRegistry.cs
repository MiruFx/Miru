using Microsoft.Extensions.DependencyInjection;
using Miru.Core;

namespace Miru.Makers;

public static class MakersRegistry
{
    internal static IServiceCollection AddMakers(this IServiceCollection services) =>
        services
            .AddSingleton<Maker>()
            // TODO: automatically scan for Miru's Consolable inside Makers namespace
            .AddConsolable<MakeAppSettingsConsolable>()
            .AddConsolable<MakeEntityConsolable>()
            .AddConsolable<MakeScaffoldConsolable>()
            .AddConsolable<MakeQueryListConsolable>()
            .AddConsolable<MakeQueryShowConsolable>()
            .AddConsolable<MakeCommandConsolable>()
            .AddConsolable<MakeMailableConsolable>()
            .AddConsolable<MakeConsolableConsolable>()
            .AddConsolable<MakeMigrationConsolable>();
}