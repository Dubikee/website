using LiteDB;
using Server.Shared.Core.Database;
using Server.Shared.Models.Auth;
using Server.Shared.Options;
using System.Collections.Generic;

namespace Server.DB.UserDb
{
    public class UserDbContext : IUserDbContext<AppUser>
    {
        private readonly LiteCollection<AppUser> _users;

        public IEnumerable<AppUser> Users => _users.FindAll();

        public UserDbContext(DbOptions opt)
        {
            _users = new LiteDatabase(opt.DbPath).GetCollection<AppUser>(opt.UserCollectionName);
        }


        /// <inheritdoc />
        /// <summary>
        /// 添加用户，User不可为null
        /// </summary>
        /// <param name="user"></param>
        public void AddUser(AppUser user)
        {
            _users.Insert(user);
        }

        /// <inheritdoc />
        /// <summary>
        /// 通过Uid找到User,uid不可为空,且不可与已有重复
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public AppUser FindUser(string uid)
        {
            return _users.FindOne(x => x.Uid == uid);
        }


        /// <inheritdoc />
        /// <summary>
        /// 更新User，User不可为null
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool UpdateUser(AppUser user)
        {
            return _users.Update(user);
        }

        /// <inheritdoc />
        /// <summary>
        /// 删除User，User不可位null
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool DeleteUser(AppUser user)
        {
            return _users.Delete(user.Id);

        }
    }
}
