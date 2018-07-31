using System;
using System.Collections.Generic;
using System.Text;
using Server.Shared.Models;
using Server.Shared.Results;

namespace Server.Shared.Core
{
    public interface IAdminManager<out TUser> where TUser : class, IUser
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        TUser FindUser(string uid);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="selecter"></param>
        /// <returns></returns>
        IEnumerable<User> FindUser(Func<TUser, bool> selecter);
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IEnumerable<User> FindAllUsers();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        DeleteUserResult DeleteUser(string uid);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="name"></param>
        /// <param name="pwd"></param>
        /// <param name="role"></param>
        /// <param name="phone"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        InsertUserResult AddUser(string uid, string name, string pwd, string role, string phone, string email);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="targetUid"></param>
        /// <param name="name"></param>
        /// <param name="phone"></param>
        /// <param name="email"></param>
        /// <param name="role"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        UpdateUserResult EditUser(string targetUid, string name, string phone, string email, string role,string pwd);
    }
}
