using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Server.DB.UserDb;
using Server.DB.WhutDb;
using Server.Shared.Core.Database;
using Server.Shared.Models.Auth;
using Server.Shared.Models.Whut;
using Server.Shared.Options;
using System;

namespace Server.Service.Extension
{
    public static class DbContextExtension
    {
        /// <summary>
        /// 注入用户数据库依赖
        /// </summary>
        /// <param name="services"></param>
        /// <param name="optionAction"></param>
        /// <returns></returns>
        public static IServiceCollection AddAppDbContext(this IServiceCollection services,
            Action<DbOptions> optionAction = null)
        {
            var opt = new DbOptions();
            optionAction?.Invoke(opt);
            services.TryAddSingleton(opt);
            services.TryAddScoped<IUserDbContext<AppUser>, UserDbContext>();
            services.AddScoped<IWhutDbContext<WhutStudent>, WhutDbContext>();
            return services;
        }
    }
}