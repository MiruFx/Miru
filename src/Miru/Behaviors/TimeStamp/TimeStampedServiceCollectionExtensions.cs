﻿using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Miru.Behaviors.BelongsToUser;
using Miru.Userfy;

namespace Miru.Behaviors.TimeStamp
{
    public static class TimeStampedServiceCollectionExtensions
    {
        public static IServiceCollection AddTimeStamped(this IServiceCollection services)
        {
            return services.AddTransient<IInterceptor, TimeStampedInterceptor>();
        }
    }
}
