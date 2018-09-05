using Server.Shared.Core.DB;

namespace Server.DB.Models
{
    public class TableDbModel : IBaseDbModel
    {
        public int Id { get; set; }
        public string Uid { get; set; }
        public string[][] Table { get; set; }
    }
}
