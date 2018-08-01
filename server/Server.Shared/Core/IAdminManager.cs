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
        /// Find User by uid
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        (RequestResult res, TUser user) FindUser(string uid);

        /// <summary>
        /// find all users
        /// </summary>
        /// <returns></returns>
        IEnumerable<TUser> FindAllUsers();

        /// <summary>
        /// delete user by id
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        RequestResult DeleteUser(string uid);

        /// <summary>
        /// add user
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
        /// 
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
