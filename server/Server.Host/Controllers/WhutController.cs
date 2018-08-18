using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NLog;
using Server.Shared.Core.Services;
using Server.Shared.Models.Whut;
using Server.Shared.Results;
using System.Threading.Tasks;

namespace Server.Host.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    public class WhutController : ControllerBase
    {
        private readonly IWhutService<WhutStudent> _whut;
        private static readonly ILogger Log = LogManager.GetCurrentClassLogger();

        public WhutController(IWhutService<WhutStudent> whut)
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
        /// 更新信息
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult UpdateInfo(string studentId, string pwd)
        {
            var status = _whut.UpdateInfo(studentId, pwd);
            Log.Info($"{Request.Path} 学号=[{studentId}] 密码=[{pwd}] => status=[{status}]");
            return Ok(new {status});
        }

        /// <summary>
        /// 获取课表
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> TimeTable()
        {
            if (_whut.Student == null)
            {
                Log.Info($"{Request.Path} status=[StudentNotFind]");
                return Ok(new
                {
                    status = WhutStatus.StudentNotFind
                });
            }

            if (_whut.Student.TimeTable != null)
            {
                Log.Info($"{Request.Path} status=[Ok]");
                return Ok(new
                {
                    status = WhutStatus.Ok,
                    timeTable = _whut.Student.TimeTable
                });
            }

            var status = await _whut.RefreshTimeTable();
            if (status == WhutStatus.Ok)
            {
                Log.Info($"{Request.Path} studentId=[{_whut.Student.StudentId}] status=[Ok]");
                return Ok(new
                {
                    status,
                    timeTable = _whut.Student.TimeTable
                });
            }

            Log.Info($"{Request.Path} status=[{status}]");
            return Ok(new {status});

        }

        /// <summary>
        /// 更新课表
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> UpdateTimeTable()
        {
            if (_whut.Student == null)
            {
                Log.Info($"{Request.Path} status=[StudentNotFind]");
                return Ok(new
                {
                    status = WhutStatus.StudentNotFind
                });
            }

            var status = await _whut.RefreshTimeTable();
            if (status == WhutStatus.Ok)
            {
                Log.Info($"{Request.Path} studentId=[{_whut.Student.StudentId}] status=[Ok]");
                return Ok(new
                {
                    status,
                    timeTable = _whut.Student.TimeTable
                });
            }

            Log.Info($"{Request.Path} status=[{status}]");
            return Ok(new {status});
        }

        /// <summary>
        /// 更新绩点排名
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> UpdateScoreRink()
        {
            if (_whut.Student == null)
            {
                Log.Info($"{Request.Path} status=[StudentNotFind]");
                return Ok(new
                {
                    status = WhutStatus.StudentNotFind
                });
            }

            var status = await _whut.RefreshScores();
            if (status != WhutStatus.Ok)
            {
                Log.Info($"{Request.Path} status=[{status}]");
                return Ok(new {status});
            }

            Log.Info($"{Request.Path} studentId=[{_whut.Student.StudentId}] status=[Ok]");
            return Ok(new
            {
                status = WhutStatus.Ok,
                scores = _whut.Student.Scores,
                rinks = _whut.Student.Rinks
            });
        }

        /// <summary>
        /// 获取绩点排名
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> ScoreRink()
        {
            if (_whut.Student == null)
            {
                Log.Info($"{Request.Path} status=[StudentNotFind]");
                return Ok(new
                {
                    status = WhutStatus.StudentNotFind
                });
            }

            if (_whut.Student.Scores == null || _whut.Student.Rinks == null)
            {

                var status = await _whut.RefreshScores();
                if (status != WhutStatus.Ok)
                {
                    Log.Info($"{Request.Path} status=[{status}]");
                    return Ok(new {status});
                }
            }

            Log.Info($"{Request.Path} studentId=[{_whut.Student.StudentId}] status=[Ok]");
            return Ok(new
            {
                status = WhutStatus.Ok,
                scores = _whut.Student.Scores,
                rinks = _whut.Student.Rinks
            });
        }
    }
}