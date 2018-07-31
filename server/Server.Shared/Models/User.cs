using System;
using System.Security.Cryptography;
using Server.Shared.Core;
using static System.Text.Encoding;

namespace Server.Shared.Models
{
    public class User : IUser
    {
        public int Id { get; set; }
        public string UId { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public byte[] PwHash { get; set; }

        /// <summary>
        ///  make Hash 
        /// </summary>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public static byte[] MakePwdHash(string pwd) => MD5.Create().ComputeHash(UTF8.GetBytes(pwd));

        public User()
        {
        }

        public User(string uid, string name, string role, string pwd, string phone = null, string email = null)
        {
            UId = uid;
            Name = name;
            Phone = phone;
            Email = email;
            Role = role;
            PwHash = MakePwdHash(pwd);
        }
    }
}
