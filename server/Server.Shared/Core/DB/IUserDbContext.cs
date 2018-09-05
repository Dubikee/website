using System.Collections.Generic;

namespace Server.Shared.Core.DB
{
    public interface IUserDbContext<TUser> where TUser : IAppUser
    {
        IEnumerable<TUser> Users { get; }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="user"></param>
        void Add(TUser user);

        /// <summary>
        /// 查找用户（通过UID）
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        TUser Find(string uid);

        /// <summary>
        /// 更新用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        bool Update(TUser user);

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        bool Delete(TUser user);
    }
}