using System.Collections.Generic;
using LiteDB;
using Server.Shared.Core.DB;
using Server.Shared.Models.Auth;
using Server.Shared.Options;

namespace Server.DB
{
    public class UserDbContext : IUserDbContext<AppUser>
    {
        private readonly LiteCollection<AppUser> _users;

        public IEnumerable<AppUser> Users => _users.FindAll();

        public UserDbContext(LiteDatabase db, DbOptions opt)
        {
            _users = db.GetCollection<AppUser>(opt.UserName);
        }


        /// <inheritdoc />
        /// <summary>
        /// 添加用户，User不可为null
        /// </summary>
        /// <param name="appUser"></param>
        public void Add(AppUser appUser)
        {
            _users.Insert(appUser);
        }

        /// <inheritdoc />
        /// <summary>
        /// 通过Uid找到User,uid不可为空,且不可与已有重复
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public AppUser Find(string uid)
        {
            return _users.FindOne(x => x.Uid == uid);
        }


        /// <inheritdoc />
        /// <summary>
        /// 更新User，User不可为null
        /// </summary>
        /// <param name="appUser"></param>
        /// <returns></returns>
        public bool Update(AppUser appUser)
        {
            return _users.Update(appUser);
        }

        /// <inheritdoc />
        /// <summary>
        /// 删除User，User不可位null
        /// </summary>
        /// <param name="appUser"></param>
        /// <returns></returns>
        public bool Delete(AppUser appUser)
        {
            return _users.Delete(appUser.Id);

        }
    }
}
