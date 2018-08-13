using Server.Shared.Core;
using System.Collections.Generic;

namespace Server.Shared.Models.Whut
{
    public class WhutStudent : IWhutStudent
    {
        public int Id { get; set; }
        public string Uid { get; set; }
        public string StudentId { get; set; }
        public string Pwd { get; set; }
        public string[,] TimeTable { get; set; }
        public IEnumerable<ScoreInfo> Scores { get; set; }
        public GPARinks Rinks { get; set; }
    }
}
