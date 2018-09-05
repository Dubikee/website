using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using Server.Shared.Core.DB;
using Server.Shared.Core.Services;
using Server.Shared.Models.Auth;
using Server.Shared.Options;
using Server.Shared.Utils;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using Server.Shared;
using static LinqPlus.Linp;
using static Server.Shared.Models.Auth.AppUserExtension;

namespace Server.Service.Auth
{
    public class AccountManager : IAccountManager<AppUser>
    {
        private AppUser _appUser;
        private readonly AuthOptions _opt;
        private readonly HttpContext _ctx;
        private readonly IUserDbContext<AppUser> _db;
        private readonly IForbiddenJwtStore _jwtStore;

        /// <inheritdoc />
        /// <summary>
        /// 当前用户
        /// </summary>
        public AppUser AppUser
        {
            get
            {
                if (_appUser != null)
                    return _appUser;
                var uid = _ctx.User.Claims
                    .FirstOrDefault(x => x.Type == _opt.UidClaimType)?.Value;
                if (uid != null)
                    _appUser = _db.Find(uid);
                return _appUser;
            }
        }

        public AccountManager(IUserDbContext<AppUser> db,
            IHttpContextAccessor accessor,
            AuthOptions opt,
            IForbiddenJwtStore jwtStore)
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
        public (Status status, string jwt) Login(string uid, string pwd)
        {
            // 空值检查
            if (AnyNullOrWhiteSpace(uid, pwd))
                return (Status.InputIllegal, null);
            // 判断Uid是否存在
            var user = _db.Find(uid);
            if (user == null)
                return (Status.UidNotFind, null);
            // 检查密码是否正确
            if (!MakePwdHash(pwd).SequenceEqual(user.PwHash))
                return (Status.PwdWrong, null);
            _appUser = user;
            return (Status.Ok, MakeJwt(uid, user.Role));
        }

        /// <summary>
        /// 登出
        /// </summary>
        /// <returns></returns>
        public bool Logout()
        {
            var jwt = _ctx.Request.Headers.GetJwt();
            if (string.IsNullOrWhiteSpace(jwt))
                throw new Exception("JWT不可能空");
            return _jwtStore.Push(jwt);
        }


        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public (Status status, string jwt) Register(UserInfos m)
        {
            if (AnyNullOrWhiteSpace(m.Uid, m.Name, m.Pwd))
                return (Status.InputIllegal, null);
            if (!Regex.IsMatch(m.Uid, _opt.UidRegex))
                return (Status.UidIllegal, null);
            if (!Regex.IsMatch(m.Pwd, _opt.PwdRegex))
                return (Status.PwdIllegal, null);
            if (_db.Find(m.Uid) != null)
                return (Status.UidHasExist, null);
            _db.Add(new AppUser(m));
            return (Status.Ok, MakeJwt(m.Uid, RoleTypes.Vistor));
        }

        /// <inheritdoc />
        /// <summary>
        /// 移除用户
        /// </summary>
        /// <returns></returns>
        public Status DeleteUser()
        {
            if (AppUser == null)
                return Status.TokenExpired;
            _db.Delete(AppUser);
            return Status.Ok;
        }

        /// <summary>
        /// UpdateInfo
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public Status UpdateInfo(UserInfos m)
        {
            if (AppUser == null)
                return Status.TokenExpired;
            if (m.Name != null)
                AppUser.Name = m.Name;
            if (m.Phone != null)
                AppUser.Phone = m.Phone;
            if (m.Email != null)
                AppUser.Email = m.Email;
            if (m.WhutId != null)
                AppUser.WhutId = m.WhutId;
            if (m.WhutPwd != null)
                AppUser.WhutPwd = m.WhutPwd;
            _db.Update(AppUser);
            return Status.Ok;
        }

        /// <inheritdoc />
        /// <summary>
        ///  更改密码
        /// </summary>
        /// <param name="oldPwd"></param>
        /// <param name="newPwd"></param>
        /// <returns></returns>
        public Status UpdatePwd(string oldPwd, string newPwd)
        {
            if (AnyNullOrWhiteSpace(oldPwd, newPwd))
                return Status.InputIllegal;
            if (!Regex.IsMatch(newPwd, _opt.PwdRegex))
                return Status.PwdIllegal;
            if (!MakePwdHash(oldPwd).SequenceEqual(AppUser.PwHash))
                return Status.PwdWrong;
            AppUser.PwHash = MakePwdHash(newPwd);
            _db.Update(AppUser);
            return Status.Ok;
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



