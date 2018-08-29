using System;
using System.Collections.Generic;
using System.Text;
using Server.Shared.Models.Whut;

namespace Server.Shared.Core
{
    public interface IStudent
    {
        string Uid { get; set; }
        string StudentId { get; set; }
        string Pwd { get; set; }
        string[][] Table { get; set; }
        IEnumerable<ScoreDetail> Scores { get; set; }
        RinkDetail Rink { get; set; }
    }
}
