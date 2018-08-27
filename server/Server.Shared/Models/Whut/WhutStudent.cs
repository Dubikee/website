using Server.Shared.Core;
using System.Collections.Generic;

namespace Server.Shared.Models.Whut
{
    public class WhutStudent : IWhutStudent
    {
        /// <summary>
        /// 数据库主键
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 用户账号
        /// </summary>
        public string Uid { get; set; }
        /// <summary>
        /// 学号
        /// </summary>
        public string StudentId { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Pwd { get; set; }
        /// <summary>
        /// 课表
        /// </summary>
        public string[][] Table { get; set; }
        /// <summary>
        /// 分数
        /// </summary>
        public IEnumerable<Score> Scores { get; set; }
        /// <summary>
        /// 绩点,排名
        /// </summary>
        public Rink Rink { get; set; }
    }
}
