using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NLog;
using Server.Host.Models;
using Server.Shared.Core.Services;
using Server.Shared.Models.Auth;
using Server.Shared.Results;

namespace Server.Host.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IAccountManager<User> _manager;
        private static readonly ILogger Log = LogManager.GetCurrentClassLogger();

        public AccountController(IAccountManager<User> manager)
        {
            _manager = manager;
        }

        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(string uid, string pwd)
        {
            var (status, jwt) = _manager.Login(uid, pwd);
            Log.Info($"{Request.Path} uid=[{uid}] pwd=[***] =>code=[{status}]");
            if (status != AuthStatus.Ok)
                return Ok(new {status});
            var (_, name, role, phone, email) = _manager.User;
            return Ok(new {status, jwt, uid, name, phone, email, role});
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Register(UserModel m)
        {
            var (status, jwt) = _manager.Register(
                uid: m.Uid,
                name: m.Name,
                pwd: m.Pwd,
                phone: m.Phone,
                email: m.Email);
            Log.Info($"{Request.Path} uid=[{m.Uid}] name=[{m.Name}]... =>code=[{status}]");
            return status == AuthStatus.Ok ? Ok(new {status, jwt}) : Ok(new {status});
        }

        /// <summary>
        /// 修改信息
        /// </summary>
        /// <param name="name"></param>
        /// <param name="phone"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpdateInfo(string name, string phone, string email)
        {
            var status = _manager.UpdateUserInfo(name, phone, email);
            Log.Info($"{Request.Path} name=[{name}] phone=[{phone}] email=[{email}] =>code=[{status}]");
            return Ok(new {status});
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="oldPwd"></param>
        /// <param name="newPwd"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpdatePwd(string oldPwd, string newPwd)
        {
            var status = _manager.UpdateUserPwd(oldPwd, newPwd);
            Log.Info($"{Request.Path} oldPwd=[{oldPwd}] newPwd=[{newPwd}] =>code=[{status}]");
            return Ok(new {status});
        }

        [HttpGet]
        public ActionResult Validate()
        {
            if (_manager.User == null)
                return Ok(new {status = AuthStatus.TokenExpired});
            Log.Info($"{Request.Path} => uid=[{_manager.User.Id}] ...");
            var (uid, name, role, phone, email) = _manager.User;
            return Ok(new {status = AuthStatus.Ok, uid, name, phone, email, role});
        }
    }
}
