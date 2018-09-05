using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Server.Shared.Options;
using System;

namespace Server.Host.Middlewares.IPLock
{
    // ReSharper disable once UnusedMember.Global
    // ReSharper disable once InconsistentNaming
    public static class IPLockerExtensions
    {

        // ReSharper disable once InconsistentNaming
        public static IApplicationBuilder UseIPLocker(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<IPLocker>();
        }

        // ReSharper disable once InconsistentNaming
        public static IServiceCollection AddIPLocker(this IServiceCollection services,
            Action<IPLockerOptions> optionAction = null)
        {
            var opt = new IPLockerOptions();
            optionAction?.Invoke(opt);
            services.AddSingleton(opt);
            return services;
        }
    }
}
