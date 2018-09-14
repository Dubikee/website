using LiteDB;
using Microsoft.Extensions.DependencyInjection;
using Server.Shared.Options;
using System;
using System.Linq;
using Server.Shared.Models.Auth;

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
            var users = liteDb.GetCollection<AppUser>(opt.UserName);
            if (users.FindOne(x => true) == null)
            {
                users.Insert(new AppUser("17607105321",
                    "罗坤",
                    "master",
                    "abcd1234",
                    "17607105321",
                    "485386599@qq.com",
                    "0121618990514",
                    "16421181150326"));
                Console.WriteLine("Create User 17607105321");
            }

            services.AddSingleton(opt);
            services.AddSingleton(liteDb);
            return services;
        }
    }
}
