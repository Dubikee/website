using Microsoft.AspNetCore.Http;
using Server.Service.JwtAuth;
using Server.Shared.Core;
using Server.Shared.Models;
using Server.Shared.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using static System.String;
namespace Server.Service.Admin
{
    public class AdminManager:IAdminManager<User>
    {
        private readonly HttpContext _ctx;
        private readonly IUserDbContext<User> _db;

        public AdminManager(IUserDbContext<User> db, IHttpContextAccessor accessor)
        {
            _db = db;
            _ctx = accessor.HttpContext;
            //var claim = _ctx.User.Claims
            //    .FirstOrDefault(x => x.Type == AccountManager.UidClaimType);
            //if (claim != null && _db.FindUser(claim.Value) != null)
            //    return;
            //_ctx.Response.StatusCode = 403;
        }

        public User FindUser(string uid)
        {
            return IsNullOrWhiteSpace(uid) ? null : _db.FindUser(uid);
        }

        public IEnumerable<User> FindUser(Func<User, bool> selecter)
        {
            return _db.Users.Where(selecter);
        }

        public IEnumerable<User> FindAllUsers()
        {
            return _db.Users;
        }

        public DeleteUserResult DeleteUser(string uid)
        {
            if (IsNullOrWhiteSpace(uid))
                return DeleteUserResult.ParamsIsEmpty;
            var u = FindUser(uid);
            if (u == null)
                return DeleteUserResult.UserNotFind;
            if (u.Role.ToLower() == "master")
                return DeleteUserResult.NotAllowed;
            return _db.DeleteUser(u) ? DeleteUserResult.Ok : DeleteUserResult.UnknownError;
        }

        public InsertUserResult AddUser(string uid, string name, string pwd, string role, string phone, string email)
        {
            if (IsNullOrWhiteSpace(uid) || IsNullOrWhiteSpace(name) || IsNullOrWhiteSpace(pwd) ||
                IsNullOrWhiteSpace(role))
                return InsertUserResult.ParamsIsEmpty;
            if (_db.FindUser(uid) != null)
                return InsertUserResult.UidHasExist;
            _db.AddUser(new User
            (
                uid: uid,
                name: name,
                role: RoleTypes.Vistor,
                pwd: pwd,
                phone: phone,
                email: email
            ));
            return InsertUserResult.Ok;
        }

        public UpdateUserResult EditUser(string targetUid, string name, string phone, string email, string role,
            string pwd)
        {
            if (IsNullOrWhiteSpace(targetUid))
                return UpdateUserResult.ParamsIsEmpty;
            var user = FindUser(targetUid);
            if (user == null)
                return UpdateUserResult.UserNotFind;
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
            return UpdateUserResult.Ok;
        }
    }
}
