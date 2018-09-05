namespace Server.Shared.Options
{
    public class DbOptions
    {
        public string DbPath { get; set; }
        public string EotName { get; set; } = "EOT";
        public string RinkName { get; set; } = "RINK";
        public string ScoresName { get; set; } = "SCORES";
        public string TableName { get; set; } = "TABLE";
        public string UserName { get; set; } = "USER";
    }
}
