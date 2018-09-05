using Server.DB;
using Server.Shared.Core.DB;
using Server.Shared.Models.Auth;

namespace Server.Service.Whut
{
    public static class WhutUtils
    {
        public static T FindOrCreate<T>(this IBaseDbContext<T> dbContext, AppUser user) where T : IBaseDbModel, new()
        {
            return FindOrCreate(dbContext, user.Uid);
        }

        public static T FindOrCreate<T>(this IBaseDbContext<T> dbContext, string uid) where T : IBaseDbModel, new()
        {
            var data = dbContext.Find(uid);
            if (data == null)
            {
                data = new T
                {
                    Uid = uid
                };
                dbContext.Add(data);
            }

            return data;
        }
    }
}
