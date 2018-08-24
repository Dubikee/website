using Microsoft.AspNetCore.Http;
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
using System.Text.RegularExpressions;
using static System.String;

namespace Server.Service.Auth
{
    public class AccountManager : IAccountManager<User>
    {
        private User _user;
        private readonly AuthOptions _opt;
        private readonly HttpContext _ctx;
        private readonly IUserDbContext<User> _db;

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
                    .FirstOrDefault(x => x.Type == _opt.UidClaimType)?.Value;
                if (uid != null)
                    _user = _db.FindUser(uid);
                return _user;
            }
        }

        public AccountManager(IUserDbContext<User> db, IHttpContextAccessor accessor, AuthOptions opt)
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
            if (IsNullOrWhiteSpace(uid) || IsNullOrWhiteSpace(pwd))
                return (AuthStatus.InputIllegal, null);
            // 判断Uid是否存在
            var user = _db.FindUser(uid);
            if (user == null)
                return (AuthStatus.UIdNotFind, null);
            // 检查密码是否正确
            if (!User.MakePwdHash(pwd).SequenceEqual(user.PwHash))
                return (AuthStatus.PasswordWrong, null);
            return (AuthStatus.Ok, MakeJwt(uid, user.Role));
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
            string phone, 
            string email)
        {
            if (IsNullOrWhiteSpace(uid) || IsNullOrWhiteSpace(name) || IsNullOrWhiteSpace(pwd))
                return (AuthStatus.InputIllegal, null);
            if (!Regex.IsMatch(uid, _opt.UidRegex))
                return (AuthStatus.UidIllegal, null);
            if (!Regex.IsMatch(pwd, _opt.PwdRegex))
                return (AuthStatus.PasswordIllegal, null);
            if (null != _db.FindUser(uid))
                return (AuthStatus.UidHasExist, null);
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
            _db.DeleteUser(User);
            return AuthStatus.Ok;
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
            var n = IsNullOrWhiteSpace(name);
            var p = IsNullOrWhiteSpace(phone);
            var e = IsNullOrWhiteSpace(email);
            if (n && p && e)
                return AuthStatus.InputIllegal;
            if (!n)
                User.Name = name;
            if (!p)
                User.Phone = phone;
            if (!e)
                User.Email = email;
            _db.UpdateUser(User);
            return AuthStatus.Ok;
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
            if (IsNullOrWhiteSpace(oldPwd) || IsNullOrWhiteSpace(newPwd))
                return AuthStatus.InputIllegal;
            if (!Regex.IsMatch(newPwd, _opt.PwdRegex))
                return AuthStatus.PasswordIllegal;
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
                new Claim(_opt.UidClaimType, uid),
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
    }
}



