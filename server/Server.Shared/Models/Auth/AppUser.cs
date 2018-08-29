using System.Security.Cryptography;
using System.Text;
using Server.Shared.Core;

namespace Server.Shared.Models.Auth
{
    public class AppUser : IUser
    {
        public int Id { get; set; }
        public string Uid { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public byte[] PwHash { get; set; }

        /// <summary>
        /// 无参构造函数
        /// </summary>
        public AppUser()
        {
        }

        /// <summary>
        /// 有参构造函数
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="name"></param>
        /// <param name="role"></param>
        /// <param name="pwd"></param>
        /// <param name="phone"></param>
        /// <param name="email"></param>
        public AppUser(
            string uid,
            string name,
            string role,
            string pwd,
            string phone = null,
            string email = null)
        {
            Uid = uid;
            Name = name;
            Phone = phone;
            Email = email;
            Role = role;
            PwHash = MakePwdHash(pwd);
        }

        /// <summary>
        ///  密码哈希加密
        /// </summary>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public static byte[] MakePwdHash(string pwd) => MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(pwd));
    }

    public static class AppUserExtension
    {
        public static void Deconstruct(
            this AppUser user,
            out string uid,
            out string name,
            out string role,
            out string phone,
            out string email)
        {
            uid = user.Uid;
            name = user.Name;
            role = user.Role;
            phone = user.Phone;
            email = user.Email;
        }
    }
}
