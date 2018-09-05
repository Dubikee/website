using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Server.DB;
using Server.DB.Models;
using Server.Shared.Core.DB;
using Server.Shared.Models.Auth;

namespace Server.Service.Extension
{
    public static class DbContextExtension
    {
        /// <summary>
        /// 注入用户数据库依赖
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddAppDbContext(this IServiceCollection services)
        {
            services.TryAddScoped<IUserDbContext<AppUser>, UserDbContext>();
            services.TryAddScoped<IBaseDbContext<EotDbModel>, EotDbContext>();
            services.TryAddScoped<IBaseDbContext<RinkDbModel>, RinkDbContext>();
            services.TryAddScoped<IBaseDbContext<ScoresDbModel>, ScoresDbContext>();
            services.TryAddScoped<IBaseDbContext<TableDbModel>, TableDbContext>();
            return services;
        }
    }
}