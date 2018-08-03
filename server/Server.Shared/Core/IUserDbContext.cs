using System.Collections.Generic;

namespace Server.Shared.Core
{
    public interface IUserDbContext<TUser> where TUser : IUser
    {
        IEnumerable<TUser> Users { get; }
        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="user"></param>
        void AddUser(TUser user);

        /// <summary>
        /// 查找用户（通过UID）
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        TUser FindUser(string uid);

        /// <summary>
        /// 更新用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        bool UpdateUser(TUser user);

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        bool DeleteUser(TUser user);
    }
}