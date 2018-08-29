namespace Server.Shared.Options
{
    public class RedisOptions
    {
        public string RedisConnection { get; set; }
        // ReSharper disable once InconsistentNaming
        public int IPDataBase { get; set; }
        public int JwtDataBase { get; set; }
    }
}