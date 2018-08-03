using Microsoft.AspNetCore.Http;
using Server.Shared.Core;
using Server.Shared.Models;
using Server.Shared.Results;
using System.Collections.Generic;
using static System.String;

namespace Server.Service.AdminService
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
        public (RequestResult res, User user) FindUser(string uid)
        {
            if (IsNullOrWhiteSpace(uid))
                return (RequestResult.ParamsIsEmpty, null);
            var u = _db.FindUser(uid);
            return u == null ? (RequestResult.UIdNotFind, null) : (RequestResult.Ok, u);
        }

        /// <inheritdoc />
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public RequestResult DeleteUser(string uid)
        {
            var (res, user) = FindUser(uid);
            if (res != RequestResult.Ok)
                return res;
            if (user.Role.ToLower() == "master")
                return RequestResult.NotAllowed;
            return _db.DeleteUser(user) ? RequestResult.Ok : RequestResult.UnknownError;
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
        public RequestResult AddUser(string uid, string name, string pwd, string role, string phone, string email)
        {
            if (IsNullOrWhiteSpace(uid) || IsNullOrWhiteSpace(name) || IsNullOrWhiteSpace(pwd) ||
                IsNullOrWhiteSpace(role))
                return RequestResult.ParamsIsEmpty;
            if (_db.FindUser(uid) != null)
                return RequestResult.UidHasExist;
            _db.AddUser(new User
            (
                uid: uid,
                name: name,
                role: RoleTypes.Vistor,
                pwd: pwd,
                phone: phone,
                email: email
            ));
            return RequestResult.Ok;
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
        public RequestResult EditUser(string targetUid, string name, string phone, string email, string role,
            string pwd)
        {
            var (res, user) = FindUser(targetUid);
            if (res != RequestResult.Ok)
                return res;
            if (IsNullOrWhiteSpace(name))
                user.Name = name;
            if (IsNullOrWhiteSpace(phone))
                user.Phone = phone;
            if (IsNullOrWhiteSpace(email))
                user.Email = email;
            if (IsNullOrWhiteSpace(role))
                user.Role = role;
            if (!IsNullOrWhiteSpace(pwd))
                user.PwHash = User.MakePwdHash(pwd);
            return RequestResult.Ok;
        }
    }
}
