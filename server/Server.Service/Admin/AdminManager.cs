using Microsoft.AspNetCore.Http;
using Server.Shared.Core;
using Server.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Server.Shared.Results;
using static System.String;

namespace Server.Service.Admin
{
    public class AdminManager : IAdminManager<User>
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

        public (RequestResult res, User user) FindUser(string uid)
        {
            if (IsNullOrWhiteSpace(uid))
                return (RequestResult.ParamsIsEmpty, null);
            var u = _db.FindUser(uid);
            return u == null ? (RequestResult.UIdNotFind, null) : (RequestResult.Ok, u);
        }

        public IEnumerable<User> FindAllUsers()
        {
            return _db.Users;
        }

        public RequestResult DeleteUser(string uid)
        {
            var (res, user) = FindUser(uid);
            if (res != RequestResult.Ok)
                return res;
            if (user.Role.ToLower() == "master")
                return RequestResult.NotAllowed;
            return _db.DeleteUser(user) ? RequestResult.Ok : RequestResult.UnknownError;
        }

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
