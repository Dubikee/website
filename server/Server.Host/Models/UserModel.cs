namespace Server.Host.Models
{
    /// <summary>
    /// 前端用户模型
    /// </summary>
    public class UserModel
    {
        public string Uid { get; set; }
        public string Name { get; set; }
        public string Pwd { get; set; }
        public string Role { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }
}