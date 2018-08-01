using Microsoft.Extensions.DependencyInjection;
using Server.Service.Admin;
using Server.Shared.Core;
using Server.Shared.Models;

namespace Server.Host.Services
{
    public static class AdminService
    {
        public static IServiceCollection AddAdminService(this IServiceCollection services)
        {
            services.AddScoped<IAdminManager<User>, AdminManager>();
            return services;
        }
    }
}
