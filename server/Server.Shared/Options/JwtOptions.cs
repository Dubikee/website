using System;

namespace Server.Shared.Options
{
    /// <summary>
    /// Jwt的配置
    /// </summary>
    public class JwtOptions
    {
        /// <summary>
        /// Key
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 颁发这者
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// 被颁发者
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        public TimeSpan Expires { get; set; }
    }
}