using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Server.Shared.Options;
using StackExchange.Redis;

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
            services.AddSingleton(redis);
            return services;
        }
    }
}
