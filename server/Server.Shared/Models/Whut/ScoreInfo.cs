namespace Server.Shared.Models.Whut
{
    public class ScoreInfo
    {
        /// <summary>
        /// 学年学期
        /// </summary>
        /// <value></value>
        public string SchoolYear { get; set; }

        /// <summary>
        /// 课程代码
        /// </summary>
        /// <value></value>
        public string CourseCode { get; set; }

        /// <summary>
        /// 课程名称
        /// </summary>
        /// <value></value>
        public string CourseName { get; set; }

        /// <summary>
        /// 课程类型
        /// </summary>
        /// <value></value>
        public string CourseType { get; set; }

        /// <summary>
        /// 学分
        /// </summary>
        /// <value></value>
        public string CourseCredit { get; set; }

        /// <summary>
        /// 总评成绩
        /// </summary>
        /// <value></value>
        public string TotalMark { get; set; }

        /// <summary>
        /// 最高成绩
        /// </summary>
        /// <value></value>
        public string BestScore { get; set; }

        /// <summary>
        /// 首次成绩
        /// </summary>
        /// <value></value>
        public string FirstScore { get; set; }

        /// <summary>
        /// 是否重修
        /// </summary>
        /// <value></value>
        public string IsRetrain { get; set; }

        /// <summary>
        /// 学分绩点
        /// </summary>
        /// <value></value>
        public string GPA { get; set; }
    }
}