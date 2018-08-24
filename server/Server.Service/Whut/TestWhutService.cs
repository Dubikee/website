using Server.Shared.Core.Services;
using Server.Shared.Models.Whut;
using Server.Shared.Results;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server.Service.Whut
{
    public class TestWhutService : IWhutService<WhutStudent>
    {
        public WhutStudent Student { get; } = new WhutStudent
        {
            StudentId = "0121618990514",
            Pwd = "**********",
            Uid = "17607105321",
            Scores = new List<Score>
            {
                new Score
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
                new Score
                {
                    SchoolYear = "2018-1",
                    CourseCode = "ss2vyb7nv99",
                    CourseName = "大学物理",
                    CourseType = "选修课",
                    CourseCredit = "5",
                    TotalMark = "80",
                    BestScore = "",
                    FirstScore = "",
                    IsRetrain = "是",
                    Gpa = "3.0"
                },
                new Score
                {
                    SchoolYear = "2018-1",
                    CourseCode = "ss2vyb7nv99",
                    CourseName = "小学物理",
                    CourseType = "必修课",
                    CourseCredit = "5",
                    TotalMark = "70",
                    BestScore = "",
                    FirstScore = "",
                    IsRetrain = "是",
                    Gpa = "2.0"
                },
                new Score
                {
                    SchoolYear = "2018-1",
                    CourseCode = "ss2vyb7nv99",
                    CourseName = "中学物理",
                    CourseType = "个性课",
                    CourseCredit = "5",
                    TotalMark = "60",
                    BestScore = "",
                    FirstScore = "",
                    IsRetrain = "",
                    Gpa = "1.0"
                },
                new Score
                {
                    SchoolYear = "2018-1",
                    CourseCode = "ss2vyb7nv99",
                    CourseName = "硕士物理",
                    CourseType = "个性课",
                    CourseCredit = "5",
                    TotalMark = "50",
                    BestScore = "",
                    FirstScore = "",
                    IsRetrain = "",
                    Gpa = "0"
                },
                new Score
                {
                    SchoolYear = "2018-1",
                    CourseCode = "ss2vyb7nv99",
                    CourseName = "博士物理",
                    CourseType = "必修课",
                    CourseCredit = "5",
                    TotalMark = "95",
                    BestScore = "",
                    FirstScore = "",
                    IsRetrain = "是",
                    Gpa = "4.5"
                },
                new Score
                {
                    SchoolYear = "2018-1",
                    CourseCode = "ss2vyb7nv99",
                    CourseName = "高等数学",
                    CourseType = "选修课",
                    CourseCredit = "5",
                    TotalMark = "80",
                    BestScore = "",
                    FirstScore = "",
                    IsRetrain = "",
                    Gpa = "3.0"
                },
                new Score
                {
                    SchoolYear = "2018-1",
                    CourseCode = "ss2vyb7nv99",
                    CourseName = "低等数学",
                    CourseType = "必修课",
                    CourseCredit = "5",
                    TotalMark = "70",
                    BestScore = "",
                    FirstScore = "",
                    IsRetrain = "",
                    Gpa = "2.0"
                },
                new Score
                {
                    SchoolYear = "2018-1",
                    CourseCode = "ss2vyb7nv99",
                    CourseName = "中等数学",
                    CourseType = "必修课",
                    CourseCredit = "5",
                    TotalMark = "60",
                    BestScore = "",
                    FirstScore = "",
                    IsRetrain = "",
                    Gpa = "1.0"
                },
                new Score
                {
                    SchoolYear = "2018-1",
                    CourseCode = "ss2vyb7nv99",
                    CourseName = "不等数学",
                    CourseType = "必修课",
                    CourseCredit = "5",
                    TotalMark = "50",
                    BestScore = "",
                    FirstScore = "",
                    IsRetrain = "",
                    Gpa = "0"
                },
                new Score
                {
                    SchoolYear = "2018-1",
                    CourseCode = "ss2vyb7nv99",
                    CourseName = "相等数学",
                    CourseType = "必修课",
                    CourseCredit = "5",
                    TotalMark = "40",
                    BestScore = "",
                    FirstScore = "",
                    IsRetrain = "",
                    Gpa = "0"
                },
                new Score
                {
                    SchoolYear = "2018-1",
                    CourseCode = "ss2vyb7nv99",
                    CourseName = "普通化学",
                    CourseType = "必修课",
                    CourseCredit = "5",
                    TotalMark = "90",
                    BestScore = "",
                    FirstScore = "",
                    IsRetrain = "",
                    Gpa = "4.0"
                },
                new Score
                {
                    SchoolYear = "2018-1",
                    CourseCode = "ss2vyb7nv99",
                    CourseName = "非普通化学",
                    CourseType = "必修课",
                    CourseCredit = "5",
                    TotalMark = "95",
                    BestScore = "",
                    FirstScore = "",
                    IsRetrain = "",
                    Gpa = "4.5"
                }
            },
            Rink = new Rink
            {
                ClassRink = "10",
                GradeRink = "10",
                PureGpa = "3.123",
                TotalGpa = "3.456"
            },
            Table = new string[5, 7]
            {
                {
                    "高等数学A(第01-16周,M老师,5-301)",
                    "大学物理A(第01-16周,W老师,1-319)",
                    "Java程序设计(第01-10周,J老师,1-319)",
                    "编译原理(第01-14单周,B老师,1-319)",
                    "电工A(第01-17周,刘明,5-301)",
                    "",
                    ""
                },
                {
                    "Java程序设计(第01-10单周,J老师,1-319) 编译原理(第01-14双周,B老师,1-319)",
                    "数据库原理(第01-10单周,D老师,5-309) 大学物理A(第01-16双周,W老师,1-119)",
                    "C++程序设计(第01-10单周,P老师,1-319)",
                    "",
                    "高等数学A(第01-16双周,M老师,5-101)",
                    "",
                    ""
                },
                {
                    "操作系统(第01-14单周,C老师,1-301) C++程序设计(第01-10双周,P老师,5-201)",
                    "数据库原理(第01-16单周,D老师,5-309)",
                    "操作系统(第01-14双周,C老师,1-301)",
                    "",
                    "编译原理(第01-14周,B老师,1-301)",
                    "",
                    ""
                },
                {
                    "",
                    "",
                    "C++程序设计(第01-10周,P老师,1-319)",
                    "",
                    "大学体育(第01-16周,A老师,余区体育)",
                    "",
                    ""
                },
                {
                    "人文物理(第01-8周,R老师,5-101)",
                    "Web技术(第01-6周,G老师,5-417)",
                    "",
                    "",
                    "",
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

        public async Task<WhutStatus> UpdateTable()
        {
            return WhutStatus.Ok;
        }

        public async Task<WhutStatus> UpdateScoresRink()
        {
            return WhutStatus.Ok;
        }

        public async Task<int> Evaluate()
        {
            return 1;
        }
    }
}
