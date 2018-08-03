using System;
using System.Collections.Generic;
using System.Text;
using Server.Shared.Models;
using Server.Shared.Results;

namespace Server.Shared.Core
{
    public interface IAdminManager<TUser> where TUser : IUser
    {
        /// <summary>
        /// Uid查询用户
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        (RequestResult res, TUser user) FindUser(string uid);

        /// <summary>
        /// 查找所有用户
        /// </summary>
        /// <returns></returns>
        IEnumerable<TUser> FindAllUsers();

        /// <summary>
        /// 通过Uid删除用户
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        RequestResult DeleteUser(string uid);

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="name"></param>
        /// <param name="pwd"></param>
        /// <param name="role"></param>
        /// <param name="phone"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        RequestResult AddUser(string uid, string name, string pwd, string role, string phone, string email);

        /// <summary>
        /// 更改用户
        /// </summary>
        /// <param name="targetUid"></param>
        /// <param name="name"></param>
        /// <param name="phone"></param>
        /// <param name="email"></param>
        /// <param name="role"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        RequestResult EditUser(string targetUid, string name, string phone, string email, string role, string pwd);
    }
}
