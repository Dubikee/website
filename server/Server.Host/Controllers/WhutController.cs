using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NLog;
using Server.Shared.Core.Services;
using Server.Shared.Models.Whut;
using Server.Shared.Results;
using System.Threading.Tasks;

namespace Server.Host.Controllers
{
    [Authorize]
    public class WhutController : ControllerBase
    {
        private readonly IWhutService<WhutStudent> _whut;
        private static readonly ILogger Log = LogManager.GetCurrentClassLogger();

        public WhutController(IWhutService<WhutStudent> whut)
        {
            _whut = whut;
        }

        [HttpPost]
        public IActionResult UpdateInfo(string studentId, string pwd)
        {
            var status = _whut.UpdateInfo(studentId, pwd);
            Log.Info($"{Request.Path} 学号=[{studentId}] 密码=[{pwd}] => status=[{status}]");
            return Ok(status);
        }


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