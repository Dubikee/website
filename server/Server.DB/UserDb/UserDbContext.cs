using System;
using System.Collections.Generic;
using LiteDB;
using Server.Shared.Core;
using Server.Shared.Models;
using Server.Shared.Options;

namespace Server.DB.UserDb
{
    public class UserDbContext : IUserDbContext<User>
    {
        private readonly LiteCollection<User> _users;

        public IEnumerable<User> Users => _users.FindAll();

        public UserDbContext(DbOptions opt)
        {
            _users = new LiteDatabase(opt.DbPath).GetCollection<User>(opt.UserCollectionName);
        }


        /// <inheritdoc />
        /// <summary>
        /// 添加用户，User不可为null
        /// </summary>
        /// <param name="user"></param>
        public void AddUser(User user)
        {
            _users.Insert(user);
        }

        /// <inheritdoc />
        /// <summary>
        /// 通过Uid找到User,uid不可为空,且不可与已有重复
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public User FindUser(string uid)
        {
            return _users.FindOne(x => x.Uid == uid);
        }


        /// <inheritdoc />
        /// <summary>
        /// 更新User，User不可为null
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool UpdateUser(User user)
        {
            return _users.Update(user);
        }

        /// <inheritdoc />
        /// <summary>
        /// 删除User，User不可位null
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool DeleteUser(User user)
        {
            return _users.Delete(user.Id);

        }
    }
}
