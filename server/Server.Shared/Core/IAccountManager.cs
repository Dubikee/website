﻿using Server.Shared.Models;
using Server.Shared.Results;

namespace Server.Shared.Core
{
    public interface IAccountManager<out TUser> where TUser : IUser
    {
        /// <summary>
        /// 用户
        /// </summary>
        TUser User { get; }

        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        (RequestResult res, string jwt) Login(string uid, string pwd);

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="uid"></param> 
        /// <param name="name"></param>
        /// <param name="pwd"></param>
        /// <param name="phone"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        (RequestResult res, string jwt) Register(string uid, string name, string pwd, string phone, string email);

        /// <summary>
        /// 删除当前用户
        /// </summary>
        /// <returns></returns>
        RequestResult DeleteUser();

        /// <summary>
        /// 更新信息
        /// </summary>
        /// <param name="name"></param>
        /// <param name="phone"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        RequestResult UpdateUserInfo(string name, string phone, string email);

        /// <summary>
        /// 更改密码
        /// </summary>
        /// <param name="oldPwd"></param>
        /// <param name="newPwd"></param>
        /// <returns></returns>
        RequestResult UpdateUserPwd(string oldPwd, string newPwd);
    }
}