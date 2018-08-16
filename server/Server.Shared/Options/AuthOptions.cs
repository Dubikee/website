using System;

namespace Server.Shared.Options
{
    /// <summary>
    /// Jwt的配置
    /// </summary>
    public class AuthOptions
    {
        /// <summary>
        /// Key
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 颁发者
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// 被颁发者
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        /// 账号正则
        /// </summary>
        public string UidRegex { get; set; }

        /// <summary>
        /// 密码正则
        /// </summary>
        public string PwdRegex { get; set; }

        /// <summary>
        /// Uid声明类型
        /// </summary>
        public string UidClaimType { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        public TimeSpan Expires { get; set; }

    }
}