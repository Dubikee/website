﻿using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using Server.Shared.Core.Database;
using Server.Shared.Core.Services;
using Server.Shared.Models.Auth;
using Server.Shared.Options;
using Server.Shared.Results;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Server.Service.Auth
{
    public class AccountManager : IAccountManager<User>
    {
        private User _user;
        private readonly JwtOptions _opt;
        private readonly HttpContext _ctx;
        private readonly IUserDbContext<User> _db;
        public static readonly string UidClaimType = "www.luokun.xyz/uid";

        /// <inheritdoc />
        /// <summary>
        /// 当前用户
        /// </summary>
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
        public (AuthStatus status, string jwt) Login(string uid, string pwd)
        {
            // 空值检查
            if (String.IsNullOrWhiteSpace(uid) || String.IsNullOrWhiteSpace(pwd))
                return (AuthStatus.ParamsIsEmpty, null);

            // 判断Uid是否存在
            var u = _db.FindUser(uid);
            if (u == null)
                return (AuthStatus.UIdNotFind, null);

            // 检查密码是否正确
            if (!User.MakePwdHash(pwd).SequenceEqual(u.PwHash))
                return (AuthStatus.PasswordWrong, null);
            _user = u;
            return (AuthStatus.Ok, MakeJwt(uid, u.Role));
        }

        /// <summary>
        /// 登出
        /// </summary>
        /// <returns></returns>
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
        public (AuthStatus status, string jwt) Register(
            string uid,
            string name,
            string pwd,
            string phone = null,
            string email = null)
        {
            // 空值检查
            if (String.IsNullOrWhiteSpace(uid) || String.IsNullOrWhiteSpace(name) || String.IsNullOrWhiteSpace(pwd))
                return (AuthStatus.ParamsIsEmpty, null);

            // Uid长度检查
            if (uid.Length < 6)
                return (AuthStatus.UidTooShort, null);
            // Uid数字检查，只能包含数字
            if (!uid.All(c => (c >= '0' && c <= '9')))
                return (AuthStatus.UidIsNotNumbers, null);

            // 密码规范检查
            if (pwd.Length < 8)
                return (AuthStatus.PasswordTooShort, null);
            var (hasLetter, hasNumber) = CheckPwd(pwd);
            if (!hasNumber)
                return (AuthStatus.PasswordNoNumbers, null);
            if (!hasLetter)
                return (AuthStatus.PasswordNoLetters, null);

            // 判断是否已存在Uid
            if (null != _db.FindUser(uid))
                return (AuthStatus.UidHasExist, null);

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
            return (AuthStatus.Ok, MakeJwt(uid, RoleTypes.Vistor));
        }

        /// <inheritdoc />
        /// <summary>
        /// 移除用户
        /// </summary>
        /// <returns></returns>
        public AuthStatus DeleteUser()
        {
            if (User == null)
                return AuthStatus.TokenExpired;
            return _db.DeleteUser(User) ? AuthStatus.Ok : AuthStatus.UnknownError;
        }

        /// <inheritdoc />
        /// <summary>
        /// 更新信息
        /// </summary>
        /// <param name="name"></param>
        /// <param name="phone"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public AuthStatus UpdateUserInfo(string name, string phone, string email)
        {
            if (User == null)
                return AuthStatus.TokenExpired;
            var n = String.IsNullOrWhiteSpace(name);
            var p = String.IsNullOrWhiteSpace(phone);
            var e = String.IsNullOrWhiteSpace(email);
            if (n && p && e)
                return AuthStatus.ParamsIsEmpty;
            if (!n)
                User.Name = name;
            if (!p)
                User.Phone = phone;
            if (!e)
                User.Email = email;
            return _db.UpdateUser(User) ? AuthStatus.Ok : AuthStatus.UnknownError;
        }

        /// <inheritdoc />
        /// <summary>
        ///  更改密码
        /// </summary>
        /// <param name="oldPwd"></param>
        /// <param name="newPwd"></param>
        /// <returns></returns>
        public AuthStatus UpdateUserPwd(string oldPwd, string newPwd)
        {
            // 空值检查
            if (string.IsNullOrWhiteSpace(oldPwd) || string.IsNullOrWhiteSpace(newPwd))
                return AuthStatus.ParamsIsEmpty;
            // 长度检查
            if (newPwd.Length < 8)
                return AuthStatus.NewPasswordTooShort;
            var (hasLetter, hasNumber) = CheckPwd(newPwd);
            // 数字检查
            if (!hasNumber)
                return AuthStatus.NewPasswordNoNumbers;
            // 字母检查
            if (!hasLetter)
                return AuthStatus.NewPasswordNoLetters;
            // 验证旧密码是否正确
            if (!User.MakePwdHash(oldPwd).SequenceEqual(User.PwHash))
                return AuthStatus.PasswordWrong;
            User.PwHash = User.MakePwdHash(newPwd);
            _db.UpdateUser(User);
            return AuthStatus.Ok;
        }

        /// <summary>
        /// 产生Jwt
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
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_opt.Key));
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
        ///  检查密码是否含有数字与字母
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

