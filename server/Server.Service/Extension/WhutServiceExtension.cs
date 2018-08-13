using Microsoft.Extensions.DependencyInjection;
using Server.Service.Whut;
using Server.Shared.Core.Services;
using Server.Shared.Models.Whut;

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
            return services.AddScoped<IWhutService<WhutStudent>, TestWhutService>();
        }
    }
}
