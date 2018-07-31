﻿using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using Server.Shared.Core;
using Server.Shared.Models;
using Server.Shared.Options;
using Server.Shared.Results;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using static System.String;
using static System.Text.Encoding;
namespace Server.Service.JwtAuth
{
    public class AccountManager : IAccountManager<User>
    {
        private User _user;
        private readonly JwtOptions _opt;
        private readonly HttpContext _ctx;
        private readonly IUserDbContext<User> _db;
        public static readonly string UidClaimType = "www.luokun.xyz/uid";

        public User User
        {
            get
            {
                if (_user != null) return _user;
                var uid = _ctx.User.Claims
                    .FirstOrDefault(x => x.Type == UidClaimType)?.Value;
                if (uid != null)
                    _user = _db.FindUser(uid);
                return _user;
            }
        }

        public AccountManager(IUserDbContext<User> db, IHttpContextAccessor accessor, JwtOptions opt)
        {
            _db = db;
            _opt = opt;
            _ctx = accessor.HttpContext;
        }

        /// <inheritdoc />
        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public (LoginResult res, string jwt) Login(string uid, string pwd)
        {
            // 空值检查
            if (IsNullOrWhiteSpace(uid) || IsNullOrWhiteSpace(pwd))
                return (LoginResult.ParamsIsEmpty, null);

            // 判断用户是否存在
            var u = _db.FindUser(uid);
            if (u == null)
                return (LoginResult.UIdNotFind, null);

            // 检查密码是否正确
            if (!User.MakePwdHash(pwd).SequenceEqual(u.PwHash))
                return (LoginResult.PasswordWrong, null);
            _user = u;
            return (LoginResult.Ok, MakeJwt(uid, u.Role));
        }

        public bool Logout()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="name"></param>
        /// <param name="pwd"></param>
        /// <param name="phone"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public (InsertUserResult res, string jwt) Register(
            string uid,
            string name,
            string pwd,
            string phone = null,
            string email = null)
        {
            // 空值检查
            if (IsNullOrWhiteSpace(uid) || IsNullOrWhiteSpace(name) || IsNullOrWhiteSpace(pwd))
                return (InsertUserResult.ParamsIsEmpty, null);

            // Uid长度检查
            if (uid.Length < 6)
                return (InsertUserResult.UidTooShort, null);
            // Uid数字检查，只能包含数字
            if (!uid.All(c => (c >= '0' && c <= '9')))
                return (InsertUserResult.UidIsNotNumbers, null);

            // 密码规范检查
            if (pwd.Length < 8)
                return (InsertUserResult.PasswordTooShort, null);
            var (hasLetter, hasNumber) = CheckPwd(pwd);
            if (!hasNumber)
                return (InsertUserResult.PasswordNoNumbers, null);
            if (!hasLetter)
                return (InsertUserResult.PasswordNoLetters, null);

            // 判断是否已存在Uid
            if (null != _db.FindUser(uid))
                return (InsertUserResult.UidHasExist, null);

            // 符合条件，创建用户，分发Jwt 
            _db.AddUser(new User
            (
                uid: uid,
                name: name,
                role: RoleTypes.Vistor,
                pwd: pwd,
                phone: phone,
                email: email
            ));
            return (InsertUserResult.Ok, MakeJwt(uid, RoleTypes.Vistor));
        }

        /// <inheritdoc />
        /// <summary>
        /// 移除用户
        /// </summary>
        /// <returns></returns>
        public DeleteUserResult DeleteUser()
        {
            if (User == null)
                return DeleteUserResult.TokenExpired;
            return _db.DeleteUser(User) ? DeleteUserResult.Ok : DeleteUserResult.UnknownError;
        }

        /// <inheritdoc />
        /// <summary>
        /// 更新信息
        /// </summary>
        /// <param name="name"></param>
        /// <param name="phone"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public UpdateUserResult UpdateUserInfo(string name, string phone, string email)
        {
            if (User == null)
                return UpdateUserResult.TokenExpired;
            var n = IsNullOrWhiteSpace(name);
            var p = IsNullOrWhiteSpace(phone);
            var e = IsNullOrWhiteSpace(email);
            if (n && p && e)
                return UpdateUserResult.ParamsIsEmpty;
            if (!n)
                User.Name = name;
            if (!p)
                User.Phone = phone;
            if (!e)
                User.Email = email;
            return _db.UpdateUser(User) ? UpdateUserResult.Ok : UpdateUserResult.UnknownError;
        }

        /// <inheritdoc />
        /// <summary>
        ///  update password
        /// </summary>
        /// <param name="oldPwd"></param>
        /// <param name="newPwd"></param>
        /// <returns></returns>
        public UpdateUserResult UpdateUserPwd(string oldPwd, string newPwd)
        {
            // 空值检查
            if (IsNullOrWhiteSpace(oldPwd) || IsNullOrWhiteSpace(newPwd))
                return UpdateUserResult.ParamsIsEmpty;
            // 长度检查
            if (newPwd.Length < 8)
                return UpdateUserResult.NewPasswordTooShort;
            var (hasLetter, hasNumber) = CheckPwd(newPwd);
            // 数字检查
            if (!hasNumber)
                return UpdateUserResult.NewPasswordNoNumbers;
            // 字母检查
            if (!hasLetter)
                return UpdateUserResult.NewPasswordNoLetters;
            // 验证旧密码是否正确
            if (!User.MakePwdHash(oldPwd).SequenceEqual(User.PwHash))
                return UpdateUserResult.PasswordWrong;
            User.PwHash = User.MakePwdHash(newPwd);
            _db.UpdateUser(User);
            return UpdateUserResult.Ok;
        }

        /// <summary>
        ///  make Jwt
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="role"></param>
        /// <returns>JWT</returns>
        private string MakeJwt(string uid, string role)
        {
            var claims = new[]
            {
                new Claim(UidClaimType, uid),
                new Claim(ClaimTypes.Role, role)
            };
            var key = new SymmetricSecurityKey(UTF8.GetBytes(_opt.Key));
            var jwt = new JwtSecurityToken(
                issuer: _opt.Issuer,
                audience: _opt.Audience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow + _opt.Expires,
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        /// <summary>
        ///  check Password 
        /// </summary>
        /// <param name="pwd"></param>
        /// <returns></returns>
        private static (bool hasLetter, bool hasNumber) CheckPwd(string pwd)
        {
            var hasNumber = pwd.Any(c => (c >= '0' && c <= '9'));
            var hasLetter = pwd.Any(c => (c <= 'Z' && c >= 'A') || (c <= 'z' && c >= 'a'));
            return (hasLetter, hasNumber);
        }
    }

}

