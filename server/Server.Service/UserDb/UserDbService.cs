using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Server.DB;
using System;
using Server.DB.UserDb;
using Server.Shared.Core;
using Server.Shared.Models;
using Server.Shared.Options;

namespace Server.Service.UserDb
{
    public static class UserDbService
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
            services.AddSingleton(opt);
            services.AddScoped<IUserDbContext<User>, UserDbContext>();
            return services;
        }
    }
}
