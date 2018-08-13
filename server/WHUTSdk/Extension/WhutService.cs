using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Server.Whut.Core;
using Server.Whut.Models;

namespace Server.Whut.Extension
{
    public static class WhutService
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
        /// 检查是否为空
        /// </summary>
        /// <param name="student"></param>
        public static void Check(WhutStudent student)
        {
            if (student == null)
                throw new Exception("学生不能为null");
            if (String.IsNullOrWhiteSpace(student.StudentId) || String.IsNullOrWhiteSpace(student.Password))
                throw new Exception("学号或密码不能为null或空格");
        }

        public static async Task<string[,]> GetTimeTableAsync(this WhutStudent student)
        {
            Check(student);
            try
            {
                var html = await WhutClient.Request(LoginUrl)
                    .Form("userName", student.StudentId)
                    .Form("password", student.Password)
                    .Form("type", "xs")
                    .PostStringAsync();
                return await html.ParseTimeTable();
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 登陆
        /// </summary>
        /// <returns></returns>
        /// <exception>WhutStudent的实例为null，或学号密码为空会报错</exception>
        public static async Task<WhutStatus> LoginAsync(this WhutStudent student)
        {
            Check(student);
            try
            {
                var res = await WhutClient.Request(LoginUrl)
                    .Form("userName", student.StudentId)
                    .Form("password", student.Password)
                    .Form("type", "xs")
                    .PostAsync();
                var cerLogin = res.Headers.GetCookie("CERLOGIN");
                if (string.IsNullOrWhiteSpace(cerLogin))
                    return WhutStatus.PwdWrong;
                student.CerLogin = cerLogin;
                return WhutStatus.Ok;
            }
            catch
            {
                return WhutStatus.Failed;
            }
        }

        /// <summary>
        /// 查询分数
        /// </summary>
        /// <returns></returns>
        /// <exception>WhutStudent的实例为null或学号密码为空会报错</exception>
        public static async Task<(IEnumerable<ScoreInfo> scores,GPARinks rinks)> GetScoresAsync(this WhutStudent student)
        {
            Check(student);
            //请求开始
            try
            {
                //检查登陆凭证是否为空
                if (string.IsNullOrWhiteSpace(student.CerLogin))
                {
                    //为空重新登陆
                    var r = await student.LoginAsync();
                    if (r != WhutStatus.Ok)
                        return default;
                }

                //得到SessionId和位置
                var (sessionid, location) = await GetSessionAndLocation(student.CerLogin, ScoreEntryUrl);
                if (sessionid == null || location == null)
                    return default;

                //分数查询主页面
                var html = await WhutClient.Request(location)
                    .Cookie(student.CerLogin)
                    .Cookie(sessionid)
                    .GetStringAsync();
                //学科学分查询
                var snkey = await html.ParseSnkeyAsync();
                if (string.IsNullOrWhiteSpace(snkey))
                    return default;
                html = await WhutClient.Request(ScoreQueryUrl)
                    .Cookie(sessionid)
                    .Form("numPerPage", "100")
                    .Form("pageNum", "1")
                    .Form("snkey", snkey)
                    .Form("xh", student.StudentId)
                    .PostStringAsync();
                var infos = await html.ParseScoresAsync();
                if (infos == null)
                    return default;

                //绩点及排名查询查询请求
                html = await WhutClient.Request(RinkQueryUrl)
                    .Cookie(sessionid)
                    .GetStringAsync();
                var rinks = await html.ParseRinksAsync();
                return (infos, rinks);
            }
            catch
            {
                return default;
            }
        }

        /// <summary>
        /// 评教模块
        /// </summary>
        /// <returns>评教课程的个数，无课程可评返回0，请求错误返回-1</returns>
        /// <exception>WhutStudent的实例为null或学号密码为空会报错</exception>
        public static async Task<int> EvaLuateAsync(this WhutStudent student, int low, int high)
        {
            Check(student);
            try
            {
                //检查登陆凭证是否为空
                if (string.IsNullOrWhiteSpace(student.CerLogin))
                {
                    //为空重新登陆
                    var r = await student.LoginAsync();
                    if (r != WhutStatus.Ok)
                        return -1;
                }

                var (sessionid, location) = await GetSessionAndLocation(student.CerLogin, EotEntryUrl);
                //必须访问模块主页，才能让JSESSIONID生效，尽管主页没什么用
                var res = await WhutClient.Request(location)
                    .Cookie(student.CerLogin)
                    .Cookie(sessionid)
                    .GetAsync();
                if (!res.IsSuccessStatusCode)
                    return -1;

                //获取评教列表
                var html = await WhutClient.Request(EotListUrl)
                    .Cookie(student.CerLogin)
                    .Cookie(sessionid)
                    .GetStringAsync();
                var eotinfos = await html.ParseEotInfoAsync();
                //if (eotinfos.All(e => e.IsEvaluate = true))
                //    return EvaLuateStatus.NoTasks;
                return 0;
            }
            catch
            {
                return -1;
            }
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