using Server.Shared.Core.Services;
using Server.Shared.Models.Auth;
using Server.Shared.Options;
using Server.Shared.Utils;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Server.Shared;
using Server.Shared.Core.DB;
using static System.String;
using static LinqPlus.Linp;
using static Server.Shared.Models.Auth.AppUserExtension;

namespace Server.Service.Auth
{
    public class AdminManager : IAdminManager<AppUser>
    {
        private readonly AuthOptions _opt;
        private readonly IUserDbContext<AppUser> _db;

        /// <inheritdoc />
        /// <summary>
        /// 所有用户
        /// </summary>
        public IEnumerable<AppUser> Users => _db.Users;

        public AdminManager(IUserDbContext<AppUser> db, AuthOptions opt)
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
        public (Status status, AppUser user) FindUser(string uid)
        {
            if (IsNullOrWhiteSpace(uid))
                return (Status.InputIllegal, null);
            var u = _db.Find(uid);
            return u == null ? (Status.UidNotFind, null) : (Status.Ok, u);
        }

        /// <inheritdoc />
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public Status DeleteUser(string uid)
        {
            if (IsNullOrWhiteSpace(uid))
                return Status.InputIllegal;
            var user = _db.Find(uid);
            if (user == null)
                return Status.UidNotFind;
            if (user.IsMaster())
                return Status.NotAllowed;
            _db.Delete(user);
            return Status.Ok;
        }

        public Status AddUser(UserInfos m)
        {
            if (AnyNullOrWhiteSpace(m.Uid, m.Name, m.Role, m.Pwd))
                return Status.InputIllegal;
            if (!Regex.IsMatch(m.Uid, _opt.UidRegex))
                return Status.UidIllegal;
            if (!Regex.IsMatch(m.Pwd, _opt.PwdRegex))
                return Status.PwdIllegal;
            if (_db.Find(m.Uid) != null)
                return Status.UidHasExist;
            _db.Add(new AppUser(m));
            return Status.Ok;
        }


        public Status EditUser(UserInfos m)
        {
            var (res, user) = FindUser(m.Uid);
            if (res != Status.Ok)
                return res;
            if (!IsNullOrWhiteSpace(m.Name))
                user.Name = m.Name;
            if (!IsNullOrWhiteSpace(m.Phone))
                user.Phone = m.Phone;
            if (!IsNullOrWhiteSpace(m.Email))
                user.Email = m.Email;
            if (!IsNullOrWhiteSpace(m.Role))
                user.Role = m.Role;
            if (!IsNullOrWhiteSpace(m.Pwd))
                user.PwHash = MakePwdHash(m.Pwd);
            return Status.Ok;
        }
    }
}
