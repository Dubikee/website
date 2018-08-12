using Microsoft.Extensions.DependencyInjection;
using Server.Service.Auth;
using Server.Shared.Core;
using Server.Shared.Models;

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
            services.AddScoped<IAdminManager<User>, AdminManager>();
            return services;
        }
    }
}
