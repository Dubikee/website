using Server.Shared.Core.Services;
using Server.Shared.Options;
using StackExchange.Redis;

namespace Server.Service.Auth
{
    public class ForbiddenJwtStore : IForbiddenJwtStore
    {
        private readonly IDatabase _db;

        public ForbiddenJwtStore(ConnectionMultiplexer redis,RedisOptions opt)
        {
            _db = redis.GetDatabase(opt.JwtDataBase);
        }

        public bool IsForbidden(string jwt)
        {
            return _db.SetContains("jwts", jwt);
        }

        public bool Push(string jwt)
        {
            return _db.SetAdd("jwts", jwt);
        }
    }
}
