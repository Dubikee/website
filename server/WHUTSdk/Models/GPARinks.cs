namespace Server.Whut.Models
{
    public class GPARinks
    {
        /// <summary>
        /// 必修课绩点
        /// </summary>
        public string PureGPA { get; set; }

        /*---其他信息---*/
        /// <summary>
        /// 所有课程的绩点
        /// </summary>
        public string TotalGPA { get; set; }

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