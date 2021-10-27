using Microsoft.Extensions.DependencyInjection;
using Miru.Consolables;
using Miru.Core;

namespace Miru.Makers;

public static class MakersRegistry
{
    internal static IServiceCollection AddMakers(this IServiceCollection services) =>
        services
            .AddSingleton<Maker>()
            .AddConsolable<MakeAppSettingsConsolable>()
            .AddConsolable<MakeEntityConsolable>()
            .AddConsolable<MakeScaffoldConsolable>()
            .AddConsolable<MakeMigrationConsolable>();
}