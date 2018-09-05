using LiteDB;
using Microsoft.Extensions.DependencyInjection;
using Server.Shared.Options;
using System;

namespace Server.Service.Extension
{
    public static class LiteDbExtension
    {
        public static IServiceCollection AddLiteDb(this IServiceCollection services,
            Action<DbOptions> optionsAction)
        {
            var opt = new DbOptions();
            optionsAction?.Invoke(opt);
            var liteDb = new LiteDatabase(opt.DbPath);
            services.AddSingleton(opt);
            services.AddSingleton(liteDb);
            return services;
        }
    }
}
