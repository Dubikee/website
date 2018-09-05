using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NLog;
using Server.Shared;
using Server.Shared.Core.Services;
using Server.Shared.Models.Auth;

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
            var ( _, name, role, phone, email, whutId, _) = user;
            Log.Info($"{Request.Path} uid=[{uid}] => user=[{user}]");
            return Ok(new {code = status, name, role, phone, email, whutId});
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
                status = Status.Ok,
                users = _manager.Users.Select(x => new
                {
                    name = x.Name,
                    role = x.Role,
                    phone = x.Phone,
                    email = x.Email,
                    whutId = x.WhutId
                })
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
        public IActionResult AddUser(UserInfos m)
        {
            var code = _manager.AddUser(m);
            Log.Info($"{Request.Path} uid=[{m.Uid}] name=[{m.Name}] role=[{m.Role}] => code=[{code}]");
            return Ok(new {code});
        }

        /// <summary>
        /// 编辑用户
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult EditUser(UserInfos m)
        {
            var code = _manager.EditUser(m);
            Log.Info($"{Request.Path} uid=[{m.Uid}] name=[{m.Name}] role=[{m.Role}] => code=[{code}]");
            return Ok(new {code});
        }
    }
}
