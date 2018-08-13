namespace Server.Whut.Core
{
    public class WhutStudent
    {
        /// <summary>
        /// 学号
        /// </summary>
        public string StudentId { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 登陆Cookie凭证
        /// </summary>
        public string CerLogin { get; set; }

        /// <summary>
        /// 无参构造函数
        /// </summary>
        public WhutStudent()
        {
        }

        /// <summary>
        /// 有参构造函数
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="password"></param>
        public WhutStudent(string studentId, string password)
        {
            StudentId = studentId;
            Password = password;
        }
    }
}
