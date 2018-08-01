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

        /// <summary>
        /// 通过Uid查询用户
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult FindUser(string uid)
        {
            var (code, user) = _manager.FindUser(uid);
            _log.Info($"{Request.Path} uid=[{uid}] => user=[{user}]");
            return Ok(new {code, user});
        }


        /// <summary>
        /// 查询所有用户
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult AllUsers()
        {
            _log.Info($"{Request.Path} => users");
            return Ok(_manager.FindAllUsers());
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult DeleteUser(string uid)
        {
            var code = _manager.DeleteUser(uid);
            _log.Info($"{Request.Path} uid=[{uid}] => code=[{code}]");
            return Ok(new {code});
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddUser(UserModel m)
        {
            var code = _manager.AddUser(m.Uid, m.Name, m.Pwd, m.Role, m.Phone, m.Email);
            _log.Info($"{Request.Path} uid=[{m.Uid}] name=[{m.Name}] role=[{m.Role}] => code=[{code}]");
            return Ok(new {code});
        }

        /// <summary>
        /// 编辑用户
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult EditUser(UserModel m)
        {
            var code = _manager.EditUser(m.Uid, m.Name, m.Phone, m.Email, m.Role, m.Pwd);
            _log.Info($"{Request.Path} uid=[{m.Uid}] name=[{m.Name}] role=[{m.Role}] => code=[{code}]");
            return Ok(new {code});
        }
    }
}
