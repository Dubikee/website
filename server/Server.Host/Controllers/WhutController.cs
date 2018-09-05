using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NLog;
using Server.Shared;
using Server.Shared.Core.Services;
using Server.Shared.Models.Auth;
using System.Threading.Tasks;
namespace Server.Host.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    public class WhutController : ControllerBase
    {
        private readonly IWhutService _whut;
        private static readonly ILogger Log = LogManager.GetCurrentClassLogger();

        public WhutController(IWhutService whut, IAccountManager<AppUser> manager)
        {
            _whut = whut;
        }

        /// <summary>
        /// 验证登陆
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Validate(string studentId, string pwd)
        {
            var status = await _whut.TryLogin(studentId, pwd);
            Log.Info($"{Request.Path} 学号=[{studentId}] 密码=[{pwd}] => status=[{status}]");
            return Ok(new {status});
        }


        /// <summary>
        /// 获取课表
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Table(bool reload = false)
        {
            var (status, table) = await _whut.GetTable(reload);
            Log.Info($"{Request.Path} status=[{status}]");
            return Ok(new {status, table});
        }


        /// <summary>
        /// 更新绩点排名
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Scores(bool reload = false)
        {
            var (status, scores) = await _whut.GetScores(reload);
            Log.Info($"{Request.Path} status=[Ok]");
            return Ok(new {status, scores});
        }

        /// <summary>
        /// 更新绩点排名
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Rink(bool reload = false)
        {
            var (status, rink) = await _whut.GetRink(reload);
            Log.Info($"{Request.Path} status=[Ok]");
            return Ok(new {status, rink});
        }
    }
}