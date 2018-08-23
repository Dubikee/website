namespace Server.Shared.Models.Whut
{
    public class Rink
    {
        /// <summary>
        /// 必修课绩点
        /// </summary>
        public string PureGpa { get; set; }

        /// <summary>
        /// 所有课程的绩点
        /// </summary>
        public string TotalGpa { get; set; }

        /// <summary>
        /// 班级排名
        /// </summary>
        public string ClassRink { get; set; }

        /// <summary>
        /// 年纪排名
        /// </summary>
        public string GradeRink { get; set; }
    }
}