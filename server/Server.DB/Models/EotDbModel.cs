using Server.Shared.Core.DB;
using Server.Shared.Models.Whut;

namespace Server.DB.Models
{
    public class EotDbModel : IBaseDbModel
    {
        public int Id { get; set; }
        public string Uid { get; set; }
        public EotDetail EotDetail { get; set; }
    }
}
