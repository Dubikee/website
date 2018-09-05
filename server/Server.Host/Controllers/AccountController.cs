using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NLog;
using Server.Shared;
using Server.Shared.Core.Services;
using Server.Shared.Models.Auth;

namespace Server.Host.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IAccountManager<AppUser> _manager;
        private static readonly ILogger Log = LogManager.GetCurrentClassLogger();

        public AccountController(IAccountManager<AppUser> manager)
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
            if (status != Status.Ok)
                return Ok(new {status});
            var (_, name, role, phone, email, whutId, _) = _manager.AppUser;
            return Ok(new {status, jwt, uid, name, phone, email, whutId, role});
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Register(UserInfos m)
        {
            var (status, jwt) = _manager.Register(m);
            Log.Info($"{Request.Path} uid=[{m.Uid}] name=[{m.Name}]... =>code=[{status}]");
            return status == Status.Ok ? Ok(new {status, jwt}) : Ok(new {status});
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpdateInfo(UserInfos m)
        {
            var status = _manager.UpdateInfo(m);
            Log.Info($"{Request.Path} name=[{m.Name}] phone=[{m.Phone}] email=[{m.Email}] =>code=[{status}]");
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
            var status = _manager.UpdatePwd(oldPwd, newPwd);
            Log.Info($"{Request.Path} oldPwd=[{oldPwd}] newPwd=[{newPwd}] =>code=[{status}]");
            return Ok(new {status});
        }

        [HttpGet]
        public ActionResult Validate()
        {
            if (_manager.AppUser == null)
                return Ok(new {status = Status.TokenExpired});
            Log.Info($"{Request.Path} => uid=[{_manager.AppUser.Id}] ...");
            var (uid, name, role, phone, email, whutId, _) = _manager.AppUser;
            return Ok(new {status = Status.Ok, uid, name, phone, email, whutId, role});
        }
    }
}
