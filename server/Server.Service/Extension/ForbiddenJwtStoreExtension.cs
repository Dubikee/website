using System;
using Microsoft.Extensions.DependencyInjection;
using Server.Service.Auth;
using Server.Shared.Core.Services;
using Server.Shared.Options;

namespace Server.Service.Extension
{
    public static class ForbiddenJwtStoreExtension
    {
        public static IServiceCollection AddForbiddenJwtStore(this IServiceCollection services,
            Action<AuthOptions> optionAction = null)
        {
            services.AddScoped<IForbiddenJwtStore, ForbiddenJwtStore>();
            return services;
        }
    }
}
