using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Host.Models;
using Server.Shared.Core;
using Server.Shared.Results;
using System.Collections.Generic;
using NLog;
using Server.Shared.Models;

namespace Server.Host.Controllers
{
    [Authorize(Policy = "MasterOnly")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminManager<User> _manager;
        private static readonly ILogger _log = LogManager.GetCurrentClassLogger();
        public AdminController(IAdminManager<User> manager)
        {
            _manager = manager;
        }

        [HttpGet]
        public ActionResult<IUser> FindUser(string uid)
        {
            var user = _manager.FindUser(uid);
            _log.Info($"{Request.Path} uid=[{uid}] => user=[{user}]");
            return Ok(user);
        }

        [HttpGet]
        public ActionResult<IEnumerable<IUser>> AllUsers()
        {
            _log.Info($"{Request.Path} => users");
            return Ok(_manager.FindAllUsers());
        }

        [HttpGet]
        public ActionResult<DeleteUserResult> DeleteUser(string uid)
        {
            var code = _manager.DeleteUser(uid);
            _log.Info($"{Request.Path} uid=[{uid}] => code=[{code}]");
            return Ok(new { code });
        }

        [HttpPost]
        public ActionResult<InsertUserResult> AddUser(UserModel m)
        {
            var code = _manager.AddUser(m.Uid, m.Name, m.Pwd, m.Role, m.Phone, m.Email);
            _log.Info($"{Request.Path} uid=[{m.Uid}] name=[{m.Name}] role=[{m.Role}] => code=[{code}]");
            return Ok(new { code });

        }

        [HttpPost]
        public ActionResult<UpdateUserResult> EditUser(UserModel m)
        {
            var code = _manager.EditUser(m.Uid, m.Name, m.Phone, m.Email, m.Role, m.Pwd);
            _log.Info($"{Request.Path} uid=[{m.Uid}] name=[{m.Name}] role=[{m.Role}] => code=[{code}]");
            return Ok(new { code });
        }
    }
}
