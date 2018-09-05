using Microsoft.Extensions.DependencyInjection;
using Server.Shared.Options;
using StackExchange.Redis;
using System;

namespace Server.Service.Extension
{
    public static class RedisExtension
    {
        public static IServiceCollection AddRedis(this IServiceCollection services,
            Action<RedisOptions> optionsAction)
        {
            var opt = new RedisOptions();
            optionsAction?.Invoke(opt);
            var redis = ConnectionMultiplexer.Connect(opt.RedisConnection);
            services.AddSingleton(opt);
            services.AddSingleton(redis);
            return services;
        }
    }
}
