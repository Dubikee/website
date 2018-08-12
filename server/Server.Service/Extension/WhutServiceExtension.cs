using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Server.Service.Whut;
using Server.Shared.Core;
using Server.Shared.Options;

namespace Server.Service.Extension
{
    public static class WhutServiceExtension
    {
        /// <summary>
        /// 注入Whut服务
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddWhutService(this IServiceCollection services)
        {
            return services.AddScoped<IWhutService, WhutService>();
        }
    }
}
