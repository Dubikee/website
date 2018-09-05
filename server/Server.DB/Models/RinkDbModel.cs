using Server.Shared.Core.DB;
using Server.Shared.Models.Whut;

namespace Server.DB.Models
{
    public class RinkDbModel : IBaseDbModel
    {
        public int Id { get; set; }
        public string Uid { get; set; }
        public RinkDetail RinkDetail { get; set; }
    }
}
