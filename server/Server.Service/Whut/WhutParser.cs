using AngleSharp.Extensions;
using AngleSharp.Parser.Html;
using LinqPlus;
using Server.Shared.Models.Whut;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Server.Service.Whut
{
    /// <summary>
    /// 用于解析教务处信息的扩展方法
    /// </summary>
    public static class WhutParser
    {
        private static readonly HtmlParser Parser = new HtmlParser();

        /// <summary>
        /// 从Html中获取分数查询的链接，从而得到query的snkey
        /// </summary>
        /// <param name="html"></param>
        /// <returns>查询不到返回空</returns>
        public static async Task<string> ParseSnkeyAsync(this string html)
        {
            if (string.IsNullOrWhiteSpace(html)) return null;
            var doc = await Parser.ParseAsync(html);
            return doc.QuerySelectorAll(".accordionContent .tree > li > ul > li > a")
                .Attr("target", "newTab")
                .FirstOrDefault()?
                .GetAttribute("href")?
                .Split("?")
                .FirstOrDefault(x => x.StartsWith("snkey"))?
                .Split("=")
                .TryIndexOf(1);
        }

        /// <summary>
        /// 课表解析
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static async Task<string[,]> ParseTable(this string html)
        {
            if (string.IsNullOrWhiteSpace(html))
                return null;
            var doc = await Parser.ParseAsync(html);
            var trs = doc.QuerySelectorAll(".kcb-wrap .mui-table tr")
                .Where(x => x.ChildElementCount == 8)
                .ToArray();
            if (trs.Length != 5)
                return null;
            var timeTable = new string[5, 7];
            trs.Each((tr, row) => tr.Children
                .Skip(1)
                .Select(td => td.InnerHtml)
                .Each((text, col) => timeTable[row, col] = text)
            );
            return timeTable;
        }

        /// <summary>
        /// 从Html中查询分数
        /// </summary>
        /// <param name="html"></param>
        /// <returns>如果html不包含分数table则返回null</returns>
        public static async Task<IEnumerable<Score>> ParseScoresAsync(this string html)
        {
            if (string.IsNullOrWhiteSpace(html))
                return null;
            var doc = await Parser.ParseAsync(html);
            var list = doc.QuerySelectorAll(".pageContent .table > tbody > tr");
            if (!list.Any())
                return null;
            return list.Where(tr => tr.ChildElementCount == 14).Select(tr =>
            {
                var arr = tr.Children.Select(td => td.InnerHtml).ToArray();
                return new Score
                {
                    SchoolYear = arr[0],
                    CourseCode = arr[1],
                    CourseName = arr[2],
                    CourseType = arr[3],
                    CourseCredit = arr[5],
                    TotalMark = arr[6],
                    BestScore = arr[8],
                    FirstScore = arr[9],
                    IsRetrain = arr[12],
                    Gpa = arr[13]
                };
            });
        }

        /// <summary>
        /// 从http headers中查询cookie
        /// </summary>
        /// <param name="headers"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetCookie(this HttpResponseHeaders headers, string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                return null;
            return headers.FirstOrDefault(x => x.Key == "Set-Cookie").Value?
                .FirstOrDefault(x => x.StartsWith(key))?
                .Split(";")
                .FirstOrDefault();
        }

        /// <summary>
        /// 从html中获取绩点,排名信息
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static async Task<Rink> ParseRinksAsync(this string html)
        {
            if (string.IsNullOrWhiteSpace(html)) return default;
            var doc = await Parser.ParseAsync(html);
            var arr = doc.QuerySelectorAll(".pageContent input")
                .Select(x => x.GetAttribute("value"))
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToArray();
            return arr.Length == 5
                ? new Rink
                {
                    PureGpa = arr[0],
                    TotalGpa = arr[1],
                    ClassRink = arr[2],
                    GradeRink = arr[4]
                }
                : null;
        }

        /// <summary>
        /// 从Html中解析评教信息
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static async Task<IEnumerable<EotInfo>> ParseEotInfoAsync(this string html)
        {
            if (string.IsNullOrWhiteSpace(html))
                return null;
            var doc = await Parser.ParseAsync(html);
            var trs = doc.QuerySelectorAll(".pageContent .table > tbody > tr");
            if (!trs.Any())
                return null;
            var infos = new LinkedList<EotInfo>();
            trs.Where(tr => tr.ChildElementCount >= 5)
                .Each(tr =>
                {
                    var eot = new EotInfo();
                    //td应该是长度为5的string数组
                    var arr = tr.Children.Select(x => x.InnerHtml).ToArray();                 
                    //获得是否评教文本
                    var eval = tr.QuerySelector("td > font")?.InnerHtml;
                    if (string.IsNullOrWhiteSpace(eval))
                        return ;
                    eot.IsEvaluate = eval == "已评";
                    //获得链接文本
                    var link = tr.QuerySelector("td > a")?.GetAttribute("href");
                    if (string.IsNullOrWhiteSpace(link))
                        return ;
                    eot.EotLink = link;
                    //解析开始评教日期
                    if (!DateTime.TryParse(arr[3], out var starTime))
                        return ;
                    eot.StartTime = starTime;
                    //解析结束评教日期
                    if (!DateTime.TryParse(arr[4], out var endTime))
                        return ;
                    eot.EndTime = endTime;
                    //加入列表
                    infos.AddLast(eot);
                });
            return infos;
        }
    }
}