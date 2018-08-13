namespace Server.Shared.Options
{
    public class DbOptions
    {
        /// <summary>
        /// 数据库位置
        /// </summary>
        public string DbPath { get; set; }

        /// <summary>
        /// User数据表名称
        /// </summary>
        public string UserCollectionName { get; set; }

        /// <summary>
        /// whut数据表名称
        /// </summary>
        public string WhutCollectionName { get; set; }
    }
}
