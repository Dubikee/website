using Server.Shared.Core.Services;
using Server.Shared.Models.Whut;
using Server.Shared.Results;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server.Service.Whut
{
    public class TestWhutService:IWhutService<WhutStudent>
    {
        public WhutStudent Student { get; } = new WhutStudent
        {
            StudentId = "0121618990514",
            Pwd = "**********",
            Uid = "17607105321",
            Scores = new List<ScoreInfo>()
            {
                new ScoreInfo
                {
                    SchoolYear = "2018-2",
                    CourseCode = "7i6tbfc8o7",
                    CourseName = "高等数学",
                    CourseType = "必修课",
                    CourseCredit = "5",
                    TotalMark = "90",
                    BestScore = "",
                    FirstScore = "",
                    IsRetrain = "",
                    Gpa = "4.0"
                },
                new ScoreInfo
                {
                    SchoolYear = "2018-1",
                    CourseCode = "ss2vyb7nv99",
                    CourseName = "大学物理",
                    CourseType = "必修课",
                    CourseCredit = "5",
                    TotalMark = "95",
                    BestScore = "",
                    FirstScore = "",
                    IsRetrain = "",
                    Gpa = "4.5"
                }
            },
            Rinks = new GpaRinks
            {
                ClassRink = "10",
                GradeRink = "10",
                PureGpa = "3.123",
                TotalGpa = "3.456"
            },
            TimeTable = new string[5, 7]
            {
                {
                    "液压及气压传动(第01-10单周,杨益,1-319) 现代测试技术C(第01-14双周,吴青,1-301)",
                    "数据库技术(第01-10周,杨益,1-319)",
                    "液压及气压传动(第01-10周,杨益,1-319)",
                    "",
                    "",
                    "",
                    ""
                },
                {
                    "液压及气压传动(第01-10单周,杨益,1-319) 现代测试技术C(第01-14双周,吴青,1-301)",
                    "",
                    "液压及气压传动(第01-10周,杨益,1-319)",
                    "",
                    "液压及气压传动(第01-10周,杨益,1-319)",
                    "",
                    ""
                },
                {
                    "",
                    "",
                    "液压及气压传动(第01-10周,杨益,1-319)",
                    "液压及气压传动(第01-10单周,杨益,1-319) 现代测试技术C(第01-14双周,吴青,1-301)",
                    "现代测试技术C(第01-14周,吴青,1-301)",
                    "",
                    ""
                },
                {
                    "液压及气压传动(第01-10周,杨益,1-319)",
                    "",
                    "",
                    "现代测试技术C(第01-14周,吴青,1-301)",
                    "",
                    "",
                    ""
                },
                {
                    "液压及气压传动(第01-10单周,杨益,1-319) 现代测试技术C(第01-14双周,吴青,1-301)",
                    "数据库技术(第01-10周,杨益,1-319)",
                    "",
                    "",
                    "液压及气压传动(第01-10周,杨益,1-319)",
                    "",
                    ""
                }
            }
        };

        public async Task<WhutStatus> TryLogin()
        {
            return WhutStatus.Ok;
        }

        public WhutStatus UpdateInfo(string studentId, string pwd)
        {
            return WhutStatus.Ok;
        }

        public async Task<WhutStatus> TryLogin(string studentId, string pwd)
        {
            return WhutStatus.Ok;
        }

        public async Task<WhutStatus> RefreshTimeTable()
        {
            return WhutStatus.Ok;
        }

        public async Task<WhutStatus> RefreshScores()
        {
            return WhutStatus.Ok;
        }

        public async Task<int> Evaluate()
        {
            return 1;
        }
    }
}
