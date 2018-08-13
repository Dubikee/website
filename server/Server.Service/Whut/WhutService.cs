﻿using Server.Shared.Core.Database;
using Server.Shared.Core.Services;
using Server.Shared.Models.Whut;
using Server.Shared.Results;
using System;
using System.Threading.Tasks;
using AngleSharp.Dom.Events;
using Server.Shared.Models.Auth;
using static System.String;

namespace Server.Service.Whut
{
    public class WhutService : IWhutService<WhutStudent>
    {
        /// <summary>
        /// 登陆地址
        /// </summary>
        public static readonly string LoginUrl = "http://sso.jwc.whut.edu.cn/Certification//login.do";

        /// <summary>
        /// 分数入口地址
        /// </summary>
        public static readonly string ScoreEntryUrl = "http://202.114.90.180/Score";

        /// <summary>
        /// 学科分数查询地址
        /// </summary>
        public static readonly string ScoreQueryUrl = "http://202.114.90.180/Score/lscjList.do";

        /// <summary>
        /// 成绩排名查询地址
        /// </summary>
        public static readonly string RinkQueryUrl = "http://202.114.90.180/Score/xftjList1.do";

        /// <summary>
        /// 评教模块入口
        /// </summary>
        public static readonly string EotEntryUrl = "http://202.114.90.180/EOT";

        /// <summary>
        /// 评教列表
        /// </summary>
        public static readonly string EotListUrl = "http://202.114.90.180/EOT/pjkcList.do";

        /// <summary>
        /// 登陆凭证
        /// </summary>
        private string CerLogin { get; set; }

        private WhutStudent _whutStudent;

        private readonly IWhutDbContext<WhutStudent> _db;
        private readonly IAccountManager<User> _manager;

        public WhutService(IWhutDbContext<WhutStudent> db, IAccountManager<User> manager)
        {
            _db = db;
            _manager = manager;
        }

        public WhutStatus UpdateInfo(string studentId, string pwd)
        {
            if (_manager.User == null)
                return WhutStatus.UserNotFind;

            if (IsNullOrWhiteSpace(studentId) || IsNullOrWhiteSpace(pwd))
                return WhutStatus.ParamsIsEmpty;

            if (Student == null)
            {
                _db.AddStudent(new WhutStudent
                {
                    StudentId = studentId,
                    Pwd = pwd
                });
                return WhutStatus.CreateStudent;
            }

            Student.StudentId = studentId;
            Student.Pwd = pwd;
            _db.UpdateStudent(Student);
            return WhutStatus.Ok;
        }

        public WhutStudent Student
        {
            get
            {
                if (_whutStudent != null) return _whutStudent;
                if (_manager.User == null) return null;
                _whutStudent = _db.FindStudent(_manager.User.Uid);
                return _whutStudent;
            }
        }

        public async Task<WhutStatus> TryLogin()
        {
            if (Student == null)
                return WhutStatus.StudentNotFind;
            try
            {
                var res = await WhutClient.Request(LoginUrl)
                    .Form("userName", Student.StudentId)
                    .Form("password", Student.Pwd)
                    .Form("type", "xs")
                    .PostAsync();
                var cerLogin = res.Headers.GetCookie("CERLOGIN");
                if (IsNullOrWhiteSpace(cerLogin))
                    return WhutStatus.PwdWrong;
                CerLogin = cerLogin;
                return WhutStatus.Ok;
            }
            catch
            {
                return WhutStatus.WhutServerCrashed;
            }
        }

        public async Task<WhutStatus> RefreshTimeTable()
        {
            if (Student == null)
                return WhutStatus.StudentNotFind;
            try
            {
                var html = await WhutClient.Request(LoginUrl)
                    .Form("userName", Student.StudentId)
                    .Form("password", Student.Pwd)
                    .Form("type", "xs")
                    .PostStringAsync();
                var timetable = await html.ParseTimeTable();
                if (timetable == null)
                    return WhutStatus.PwdWrong;
                Student.TimeTable = timetable;
                return WhutStatus.Ok;
            }
            catch
            {
                return WhutStatus.WhutServerCrashed;
            }
        }

        public async Task<WhutStatus> RefreshScores()
        {
            if (Student == null)
                return WhutStatus.StudentNotFind;
            try
            {
                //检查登陆凭证是否为空
                if (IsNullOrWhiteSpace(CerLogin))
                {
                    //为空重新登陆
                    var r = await TryLogin();
                    if (r != WhutStatus.Ok)
                        return r;
                }

                //得到SessionId和位置
                var (sessionid, location) = await GetSessionAndLocation(CerLogin, ScoreEntryUrl);
                if (sessionid == null || location == null)
                    return WhutStatus.UnknownError;

                //分数查询主页面
                var html = await WhutClient.Request(location)
                    .Cookie(CerLogin)
                    .Cookie(sessionid)
                    .GetStringAsync();
                //学科学分查询
                var snkey = await html.ParseSnkeyAsync();
                if (IsNullOrWhiteSpace(snkey))
                    return WhutStatus.PwdWrong;
                html = await WhutClient.Request(ScoreQueryUrl)
                    .Cookie(sessionid)
                    .Form("numPerPage", "100")
                    .Form("pageNum", "1")
                    .Form("snkey", snkey)
                    .Form("xh", Student.StudentId)
                    .PostStringAsync();
                var scores = await html.ParseScoresAsync();
                if (scores == null)
                    return WhutStatus.WhutServerCrashed;

                //绩点及排名查询查询请求
                html = await WhutClient.Request(RinkQueryUrl)
                    .Cookie(sessionid)
                    .GetStringAsync();
                var rinks = await html.ParseRinksAsync();
                if (rinks == null)
                    return WhutStatus.WhutServerCrashed;
                Student.Scores = scores;
                Student.Rinks = rinks;
                return WhutStatus.Ok;
            }
            catch
            {
                return WhutStatus.WhutServerCrashed;
            }

        }

        public Task<int> Evaluate()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 四次重定向获得SessionId和Location
        /// </summary>
        /// <param name="cerlogin"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        private static async Task<(string sessionid, Uri location)> GetSessionAndLocation(string cerlogin, string url)
        {
            //第一次请求
            var res = await WhutClient.Request(url)
                .Cookie(cerlogin)
                .GetAsync();
            if (res.Headers.Location == null) return default;

            //第二次请求
            res = await WhutClient.Request(res.Headers.Location)
                .Cookie(cerlogin)
                .GetAsync();
            var sessionid = res.Headers.GetCookie("JSESSIONID");
            if (res.Headers.Location == null || sessionid == null) return default;

            //第三次请求
            res = await WhutClient.Request(res.Headers.Location)
                .Cookie(cerlogin)
                .GetAsync();
            if (res.Headers.Location == null) return default;

            //第四次请求
            res = await WhutClient.Request(res.Headers.Location)
                .Cookie(cerlogin)
                .GetAsync();
            return (sessionid, res.Headers.Location);
        }

    }
}
