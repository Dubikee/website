using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NLog;
using Server.Host.Models;
using Server.Shared.Core.Services;
using Server.Shared.Models.Auth;
using Server.Shared.Results;

namespace Server.Host.Controllers
{
    [Authorize(Policy = "MasterOnly")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminManager<AppUser> _manager;
        private static readonly ILogger Log = LogManager.GetCurrentClassLogger();

        public AdminController(IAdminManager<AppUser> manager)
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

            var (status, user) = _manager.FindUser(uid);
            Log.Info($"{Request.Path} uid=[{uid}] => user=[{user}]");
            return Ok(new {code = status, user});
        }

        /// <summary>
        /// 查询所有用户
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult AllUsers()
        {
            Log.Info($"{Request.Path} => users");
            return Ok(new
            {
                status = AuthStatus.Ok,
                users = _manager.Users
            });
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
            Log.Info($"{Request.Path} uid=[{uid}] => code=[{code}]");
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
            var code = _manager.AddUser(
                uid: m.Uid,
                name: m.Name,
                role: m.Role,
                pwd: m.Pwd,
                phone: m.Phone,
                email: m.Email
            );
            Log.Info($"{Request.Path} uid=[{m.Uid}] name=[{m.Name}] role=[{m.Role}] => code=[{code}]");
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
            var code = _manager.EditUser(
                uid: m.Uid,
                name: m.Name,
                role: m.Role,
                pwd: m.Pwd,
                phone: m.Phone,
                email: m.Email
            );
            Log.Info($"{Request.Path} uid=[{m.Uid}] name=[{m.Name}] role=[{m.Role}] => code=[{code}]");
            return Ok(new {code});
        }
    }
}
