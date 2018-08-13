using Microsoft.AspNetCore.Http;
using Server.Shared.Core.Database;
using Server.Shared.Core.Services;
using Server.Shared.Models.Auth;
using Server.Shared.Results;
using System;
using System.Collections.Generic;

namespace Server.Service.Auth
{
    public class AdminManager : IAdminManager<User>
    {
        private readonly HttpContext _ctx;
        private readonly IUserDbContext<User> _db;

        /// <inheritdoc />
        /// <summary>
        /// 所有用户
        /// </summary>
        public IEnumerable<User> Users => _db.Users;

        public AdminManager(IUserDbContext<User> db, IHttpContextAccessor accessor)
        {
            _db = db;
            _ctx = accessor.HttpContext;
        }

        /// <inheritdoc />
        /// <summary>
        /// Uid查询用户
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public (AuthStatus status, User user) FindUser(string uid)
        {
            if (string.IsNullOrWhiteSpace(uid))
                return (AuthStatus.ParamsIsEmpty, null);
            var u = _db.FindUser(uid);
            return u == null ? (AuthStatus.UIdNotFind, null) : (AuthStatus.Ok, u);
        }

        /// <inheritdoc />
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public AuthStatus DeleteUser(string uid)
        {
            var (res, user) = FindUser(uid);
            if (res != AuthStatus.Ok)
                return res;
            if (user.Role.ToLower() == "master")
                return AuthStatus.NotAllowed;
            return _db.DeleteUser(user) ? AuthStatus.Ok : AuthStatus.UnknownError;
        }

        /// <inheritdoc />
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
        public AuthStatus AddUser(string uid, string name, string pwd, string role, string phone, string email)
        {
            if (String.IsNullOrWhiteSpace(uid) || String.IsNullOrWhiteSpace(name) || String.IsNullOrWhiteSpace(pwd) ||
                String.IsNullOrWhiteSpace(role))
                return AuthStatus.ParamsIsEmpty;
            if (_db.FindUser(uid) != null)
                return AuthStatus.UidHasExist;
            _db.AddUser(new User
            (
                uid: uid,
                name: name,
                role: RoleTypes.Vistor,
                pwd: pwd,
                phone: phone,
                email: email
            ));
            return AuthStatus.Ok;
        }

        /// <inheritdoc />
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
        public AuthStatus EditUser(string targetUid, string name, string phone, string email, string role,
            string pwd)
        {
            var (res, user) = FindUser(targetUid);
            if (res != AuthStatus.Ok)
                return res;
            if (String.IsNullOrWhiteSpace(name))
                user.Name = name;
            if (String.IsNullOrWhiteSpace(phone))
                user.Phone = phone;
            if (String.IsNullOrWhiteSpace(email))
                user.Email = email;
            if (String.IsNullOrWhiteSpace(role))
                user.Role = role;
            if (!String.IsNullOrWhiteSpace(pwd))
                user.PwHash = User.MakePwdHash(pwd);
            return AuthStatus.Ok;
        }
    }
}
