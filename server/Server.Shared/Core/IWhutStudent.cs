using System;
using System.Collections.Generic;
using System.Text;
using Server.Shared.Models.Whut;

namespace Server.Shared.Core
{
    public interface IWhutStudent
    {
        string Uid { get; set; }
        string StudentId { get; set; }
        string Pwd { get; set; }
        string[,] TimeTable { get; set; }
        IEnumerable<ScoreInfo> Scores { get; set; }
        GPARinks Rinks { get; set; }
    }
}
