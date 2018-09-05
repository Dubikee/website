using Server.DB.Models;
using Server.Shared;
using Server.Shared.Core.DB;
using Server.Shared.Core.Services;
using Server.Shared.Models.Auth;
using Server.Shared.Models.Whut;
using Server.Shared.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NLog;
using static LinqPlus.Linp;
using static System.String;

namespace Server.Service.Whut
{
    public class WhutService : IWhutService
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

        private readonly IAccountManager<AppUser> _manager;
        private readonly IBaseDbContext<EotDbModel> _eotDb;
        private readonly IBaseDbContext<RinkDbModel> _rinkDb;
        private readonly IBaseDbContext<ScoresDbModel> _scoresDb;
        private readonly IBaseDbContext<TableDbModel> _tableDb;

        public AppUser AppUser => _manager.AppUser;

        public WhutService(IAccountManager<AppUser> manager,
            IBaseDbContext<EotDbModel> eotDb,
            IBaseDbContext<RinkDbModel> rinkDb,
            IBaseDbContext<ScoresDbModel> scoresDb,
            IBaseDbContext<TableDbModel> tableDb)
        {
            _manager = manager;
            _eotDb = eotDb;
            _rinkDb = rinkDb;
            _scoresDb = scoresDb;
            _tableDb = tableDb;
        }


        /// <summary>
        /// 此学生登陆尝试
        /// </summary>
        /// <returns></returns>
        public async Task<Status> TryLogin()
        {
            if (AppUser == null)
                return Status.TokenExpired;
            if (AnyNullOrWhiteSpace(AppUser.WhutId, AppUser.WhutPwd))
                return Status.WhutIdNotFind;
            try
            {
                var res = await LoginUrl
                    .Form("userName", AppUser.WhutId)
                    .Form("password", AppUser.WhutPwd)
                    .Form("type", "xs")
                    .PostAsync();
                if (res.IsSuccessStatusCode)
                {
                    var cerLogin = res.Headers.GetCookie("CERLOGIN");
                    if (IsNullOrWhiteSpace(cerLogin))
                        return Status.WhutPwdWrong;
                    CerLogin = cerLogin;
                    return Status.Ok;
                }

                return Status.WhutCrashed;
            }
            catch
            {
                return Status.WhutCrashed;
            }
        }

        /// <summary>
        /// 账号密码登陆尝试
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public async Task<Status> TryLogin(string studentId, string pwd)
        {
            try
            {
                var res = await LoginUrl
                    .Form("userName", studentId)
                    .Form("password", pwd)
                    .Form("type", "xs")
                    .PostAsync();
                if (res.IsSuccessStatusCode)
                {
                    var cerLogin = res.Headers.GetCookie("CERLOGIN");
                    if (IsNullOrWhiteSpace(cerLogin))
                        return Status.WhutPwdWrong;
                    CerLogin = cerLogin;
                    return Status.Ok;
                }

                return Status.WhutCrashed;

            }
            catch
            {
                return Status.WhutCrashed;
            }
        }

        /// <summary>
        /// 刷新课表
        /// </summary>
        /// <returns></returns>
        public async Task<(Status, string[][])> GetTable(bool reload = false)
        {
            try
            {
                if (AppUser == null)
                    return (Status.TokenExpired, null);
                if (AnyNullOrWhiteSpace(AppUser.WhutId, AppUser.WhutPwd))
                    return (Status.WhutIdNotFind, null);
                var model = _tableDb.FindOrCreate(AppUser);
                if (reload || model.Table == null)
                {
                    var res = await TryLogin();
                    if (res != Status.Ok)
                        return (res, null);
                    var html = await LoginUrl
                        .Form("userName", AppUser.WhutId)
                        .Form("password", AppUser.WhutPwd)
                        .Form("type", "xs")
                        .PostBodyAsync();
                    var tb = await html.ParseTable();
                    if (tb == null)
                        return (Status.WhutPwdWrong, null);
                    model.Table = tb;
                    _tableDb.Update(model);
                    return (Status.Ok, tb);
                }

                return (Status.Ok, model.Table);
            }
            catch
            {
                return (Status.WhutCrashed, null);
            }
        }

        /// <summary>
        /// 刷新分数
        /// </summary>
        /// <returns></returns>
        public async Task<(Status, IEnumerable<ScoresDetail>)> GetScores(bool reload = false)
        {
           try
            {
                if (AppUser == null)
                    return (Status.TokenExpired, null);
                if (AnyNullOrWhiteSpace(AppUser.WhutId, AppUser.WhutPwd))
                    return (Status.WhutIdNotFind, null);
                var model = _scoresDb.FindOrCreate(AppUser);
                if (reload || model.ScoresDetails == null)
                {
                    var res = await TryLogin();
                    if (res != Status.Ok)
                        return (res, null);
                    //得到SessionId和位置
                    var (sessionid, location) = await GetSessionAndLocation(CerLogin, ScoreEntryUrl);
                    if (sessionid == null || location == null)
                        return (Status.WhutCrashed, null);

                    //分数查询主页面
                    var html = await location
                        .Cookie(CerLogin)
                        .Cookie(sessionid)
                        .GetBodyAsync();
                    //学科学分查询
                    var snkey = await html.ParseSnkeyAsync();
                    if (IsNullOrWhiteSpace(snkey))
                        return (Status.WhutPwdWrong, null);
                    html = await ScoreQueryUrl
                        .Cookie(sessionid)
                        .Form("numPerPage", "100")
                        .Form("pageNum", "1")
                        .Form("snkey", snkey)
                        .Form("xh", AppUser.WhutId)
                        .PostBodyAsync();
                    var scores = await html.ParseScoresAsync();
                    if (scores == null)
                        return (Status.WhutPwdWrong, null);
                    model.ScoresDetails = scores;
                    _scoresDb.Update(model);
                    return (Status.Ok, scores);
                }

                return (Status.Ok, model.ScoresDetails);
            }
            catch
            {
                return (Status.WhutCrashed, null);
            }
        }


        public async Task<(Status, RinkDetail)> GetRink(bool reload = false)
        {
            try
            {
                if (AppUser == null)
                    return (Status.TokenExpired, null);
                if (AnyNullOrWhiteSpace(AppUser.WhutId, AppUser.WhutPwd))
                    return (Status.WhutIdNotFind, null);
                var model = _rinkDb.FindOrCreate(AppUser);
                if (reload || model.RinkDetail == null)
                {
                    var res = await TryLogin();
                    if (res != Status.Ok)
                        return (res, null);
                    //得到SessionId和位置
                    var (sessionid, location) = await GetSessionAndLocation(CerLogin, ScoreEntryUrl);
                    if (sessionid == null || location == null)
                        return (Status.WhutPwdWrong, null);

                    //分数查询主页面
                    var html = await location
                        .Cookie(CerLogin)
                        .Cookie(sessionid)
                        .GetBodyAsync();
                    //学科学分查询
                    var snkey = await html.ParseSnkeyAsync();
                    if (IsNullOrWhiteSpace(snkey))
                        return (Status.WhutPwdWrong, null);

                    //绩点及排名查询查询请求
                    html = await RinkQueryUrl
                        .Cookie(sessionid)
                        .GetBodyAsync();
                    var rink = await html.ParseRinksAsync();
                    if (rink == null)
                        return (Status.WhutCrashed, null);
                    model.RinkDetail = rink;
                    _rinkDb.Update(model);
                    return (Status.Ok, rink);
                }

                return (Status.Ok, model.RinkDetail);
            }
            catch
            {
                return (Status.WhutCrashed, null);
            }
        }

        /// <summary>
        /// 评教
        /// </summary>
        /// <returns></returns>
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
            var res = await url.Cookie(cerlogin).GetAsync();
            if (res.Headers.Location == null)
                return default;

            //第二次请求
            res = await res.Headers.Location.Cookie(cerlogin).GetAsync();
            var sessionid = res.Headers.GetCookie("JSESSIONID");
            if (res.Headers.Location == null || sessionid == null)
                return default;

            //第三次请求
            res = await res.Headers.Location.Cookie(cerlogin).GetAsync();
            if (res.Headers.Location == null)
                return default;

            //第四次请求
            res = await res.Headers.Location.Cookie(cerlogin).GetAsync();
            return (sessionid, res.Headers.Location);
        }

    }
}

