using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NLog;
using Server.Host.Models;
using Server.Shared.Core;
using Server.Shared.Models;

namespace Server.Host.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IAccountManager<User> _manager;
        private static readonly ILogger _log = LogManager.GetCurrentClassLogger();

        public AccountController(IAccountManager<User> manager)
        {
            _manager = manager;
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(string uid, string pwd)
        {
            var (code, jwt) = _manager.Login(uid, pwd);
            _log.Info($"{Request.Path} uid=[{uid}] pwd=[***] =>code=[{code}]");
            return Ok(new { code, jwt });
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Register(UserModel m)
        {
            var (code, jwt) = _manager.Register(
                uid: m.Uid,
                name: m.Name,
                pwd: m.Pwd,
                phone: m.Phone,
                email: m.Email);
            _log.Info($"{Request.Path} uid=[{m.Uid}] name=[{m.Name}]... =>code=[{code}]");

            return Ok(new { code, jwt });
        }

        [HttpPost]
        public ActionResult ChangeInfo(string name, string phone, string email)
        {
            var code = _manager.UpdateUserInfo(name, phone, email);
            _log.Info($"{Request.Path} name=[{name}] phone=[{phone}] email=[{email}] =>code=[{code}]");
            return Ok(new { code });
        }

        [HttpPost]
        public ActionResult ChangePwd(string oldPwd, string newPwd)
        {
            var code = _manager.UpdateUserPwd(oldPwd, newPwd);
            _log.Info($"{Request.Path} oldPwd=[{oldPwd}] newPwd=[{newPwd}] =>code=[{code}]");
            return Ok(new { code });
        }

        [HttpPost]
        public ActionResult Validate()
        {
            if (_manager.User == null)
                return Unauthorized();
            _log.Info($"{Request.Path} => uid=[{_manager.User.Id}] ...");
            return Ok(new
            {
                uid = _manager.User.UId,
                name = _manager.User.Name,
                phone = _manager.User.Phone,
                email = _manager.User.Email,
                role = _manager.User.Role
            });
        }
    }
}
