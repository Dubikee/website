using Server.Shared.Core.Database;
using Server.Shared.Core.Services;
using Server.Shared.Models.Auth;
using Server.Shared.Options;
using Server.Shared.Results;
using Server.Shared.Utils;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using static System.String;

namespace Server.Service.Auth
{
    public class AdminManager : IAdminManager<User>
    {
        private readonly AuthOptions _opt;
        private readonly IUserDbContext<User> _db;

        /// <inheritdoc />
        /// <summary>
        /// 所有用户
        /// </summary>
        public IEnumerable<User> Users => _db.Users;

        public AdminManager(IUserDbContext<User> db, AuthOptions opt)
        {
            _db = db;
            _opt = opt;
        }

        /// <inheritdoc />
        /// <summary>
        /// Uid查询用户
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public (AuthStatus status, User user) FindUser(string uid)
        {
            if (IsNullOrWhiteSpace(uid))
                return (AuthStatus.InputIllegal, null);
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
            if (IsNullOrWhiteSpace(uid))
                return AuthStatus.InputIllegal;
            var user = _db.FindUser(uid);
            if (user == null)
                return AuthStatus.UIdNotFind;
            if (user.IsMaster())
                return AuthStatus.NotAllowed;
            _db.DeleteUser(user);
            return AuthStatus.Ok;
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
        public AuthStatus AddUser(string uid, string name, string role, string pwd, string phone, string email)
        {
            if (IsNullOrWhiteSpace(uid) ||
                IsNullOrWhiteSpace(name) ||
                IsNullOrWhiteSpace(pwd) ||
                IsNullOrWhiteSpace(role))
                return AuthStatus.InputIllegal;
            if (!Regex.IsMatch(uid, _opt.UidRegex))
                return AuthStatus.UidIllegal;
            if (!Regex.IsMatch(pwd, _opt.PwdRegex))
                return AuthStatus.PasswordIllegal;
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
        /// <param name="uid"></param>
        /// <param name="name"></param>
        /// <param name="phone"></param>
        /// <param name="email"></param>
        /// <param name="role"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public AuthStatus EditUser(string uid, string name, string role, string pwd, string phone, string email)
        {
            var (res, user) = FindUser(uid);
            if (res != AuthStatus.Ok)
                return res;
            if (!IsNullOrWhiteSpace(name))
                user.Name = name;
            if (!IsNullOrWhiteSpace(phone))
                user.Phone = phone;
            if (!IsNullOrWhiteSpace(email))
                user.Email = email;
            if (!IsNullOrWhiteSpace(role))
                user.Role = role;
            if (!IsNullOrWhiteSpace(pwd))
                user.PwHash = User.MakePwdHash(pwd);
            return AuthStatus.Ok;
        }
    }
}
