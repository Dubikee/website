using System.Collections.Generic;
using Server.Shared.Core.DB;
using Server.Shared.Models.Whut;

namespace Server.DB.Models
{
    public class ScoresDbModel : IBaseDbModel
    {
        public int Id { get; set; }
        public string Uid { get; set; }
        public IEnumerable<ScoresDetail> ScoresDetails { get; set; }
    }
}
