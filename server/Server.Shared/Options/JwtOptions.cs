using System;

namespace Server.Shared.Options
{
    /// <summary>
    /// Json Web Token的配置
    /// </summary>
    public class JwtOptions
    {
        public string Key { get; set; }

        public string Issuer { get; set; }

        public string Audience { get; set; }

        public TimeSpan Expires { get; set; }
    }
}