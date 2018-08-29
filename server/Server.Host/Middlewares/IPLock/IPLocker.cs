using Microsoft.AspNetCore.Http;
using Server.Shared.Options;
using StackExchange.Redis;
using System.Threading.Tasks;

namespace Server.Host.Middlewares.IPLock
{
    // ReSharper disable once InconsistentNaming
    public class IPLocker
    {
        private readonly RequestDelegate _next;

        public IPLocker(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext, ConnectionMultiplexer redis, RedisOptions redisOpt,
            IPLockerOptions lockerOpt)
        {
            var db = redis.GetDatabase(redisOpt.IPDataBase);
            var ip = httpContext.Connection.RemoteIpAddress.GetAddressBytes();
            if (db.KeyExists(ip))
            {
                var time = db.StringIncrement(ip);
                if (time == lockerOpt.MaxVisitsTimes)
                {
                    db.KeyExpire(ip, lockerOpt.LockedTime);
                }
                else if (time < lockerOpt.MaxVisitsTimes)
                {
                    return _next(httpContext);
                }

                httpContext.Response.StatusCode = StatusCodes.Status423Locked;
                return Task.CompletedTask;
            }

            db.StringSet(ip, 1, lockerOpt.LimitTime);
            return _next(httpContext);
        }
    }
}
