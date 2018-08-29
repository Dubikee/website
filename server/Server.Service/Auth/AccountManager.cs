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
using Server.Shared.Utils;
using static System.String;
using static LinqPlus.Linp;

namespace Server.Service.Auth
{
    public class AccountManager : IAccountManager<AppUser>
    {
        private AppUser _user;
        private readonly AuthOptions _opt;
        private readonly HttpContext _ctx;
        private readonly IUserDbContext<AppUser> _db;
        private readonly IForbiddenJwtStore _jwtStore;

        /// <inheritdoc />
        /// <summary>
        /// 当前用户
        /// </summary>
        public AppUser User
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

        public AccountManager(IUserDbContext<AppUser> db, IHttpContextAccessor accessor, AuthOptions opt, IForbiddenJwtStore jwtStore)
        {
            _db = db;
            _opt = opt;
            _jwtStore = jwtStore;
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
            if (AnyNullOrWhiteSpace(uid, pwd))
                return (AuthStatus.InputIllegal, null);
            // 判断Uid是否存在
            var user = _db.FindUser(uid);
            if (user == null)
                return (AuthStatus.UIdNotFind, null);
            // 检查密码是否正确
            if (!AppUser.MakePwdHash(pwd).SequenceEqual(user.PwHash))
                return (AuthStatus.PasswordWrong, null);
            _user = user;
            return (AuthStatus.Ok, MakeJwt(uid, user.Role));
        }

        /// <summary>
        /// 登出
        /// </summary>
        /// <returns></returns>
        public bool Logout()
        {
            var jwt = _ctx.Request.Headers.GetJwt();
            if (string.IsNullOrWhiteSpace(jwt))
                throw new Exception("JWT不可能不空");
            return _jwtStore.Push(jwt);
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
            if (AnyNullOrWhiteSpace(uid, name, pwd))
                return (AuthStatus.InputIllegal, null);
            if (!Regex.IsMatch(uid, _opt.UidRegex))
                return (AuthStatus.UidIllegal, null);
            if (!Regex.IsMatch(pwd, _opt.PwdRegex))
                return (AuthStatus.PasswordIllegal, null);
            if (_db.FindUser(uid) != null)
                return (AuthStatus.UidHasExist, null);
            _db.AddUser(new AppUser
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
            if (AllNullOrWhiteSpace(name, phone, email))
                return AuthStatus.InputIllegal;
            if (User == null)
                return AuthStatus.TokenExpired;
            if (!IsNullOrWhiteSpace(name))
                User.Name = name;
            if (!IsNullOrWhiteSpace(phone))
                User.Phone = phone;
            if (!IsNullOrWhiteSpace(email))
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
            if (AnyNullOrWhiteSpace(oldPwd, newPwd))
                return AuthStatus.InputIllegal;
            if (!Regex.IsMatch(newPwd, _opt.PwdRegex))
                return AuthStatus.PasswordIllegal;
            if (!AppUser.MakePwdHash(oldPwd).SequenceEqual(User.PwHash))
                return AuthStatus.PasswordWrong;
            User.PwHash = AppUser.MakePwdHash(newPwd);
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



