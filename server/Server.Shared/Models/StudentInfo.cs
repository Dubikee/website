using System;
using System.Collections.Generic;
using System.Text;
using Server.Whut.Models;

namespace Server.Shared.Models
{
    public class StudentInfo
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
