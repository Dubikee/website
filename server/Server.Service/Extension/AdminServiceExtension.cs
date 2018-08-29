using Microsoft.Extensions.DependencyInjection;
using Server.Service.Auth;
using Server.Shared.Core.Services;
using Server.Shared.Models.Auth;

namespace Server.Service.Extension
{
    public static class AdminServiceExtension
    {
        /// <summary>
        /// 注入AdminManager
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddAdminService(this IServiceCollection services)
        {
            services.AddScoped<IAdminManager<AppUser>, AdminManager>();
            return services;
        }
    }
}
