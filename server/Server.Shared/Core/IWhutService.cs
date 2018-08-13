using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Server.Shared.Models;
using Server.Whut.Models;

namespace Server.Shared.Core
{
    public interface IWhutService
    {
        WhutStatus UpdateInfo(string studentId, string pwd);
        StudentInfo StudentInfo { get; }
        Task<WhutStatus> TryLogin();
        Task<WhutStatus> RefreshTimeTable();
        Task<WhutStatus> RefreshScores();
        Task<int> Evaluate();
    }
}
