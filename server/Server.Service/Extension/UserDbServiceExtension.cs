using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Server.DB.UserDb;
using Server.Shared.Core;
using Server.Shared.Models;
using Server.Shared.Options;

namespace Server.Service.Extension
{
    public static class UserDbServiceExtension
    {
        /// <summary>
        /// 注入用户数据库依赖
        /// </summary>
        /// <param name="services"></param>
        /// <param name="optionAction"></param>
        /// <returns></returns>
        public static IServiceCollection AddUserDbContext(this IServiceCollection services, Action<DbOptions> optionAction = null)
        {
            var opt = new DbOptions();
            optionAction?.Invoke(opt);
            services.TryAddSingleton(opt);
            services.TryAddScoped<IUserDbContext<User>, UserDbContext>();
            return services;
        }
    }
}
