using Server.Shared.Models.Whut;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server.Shared.Core.Services
{
    public interface IWhutService
    {
        Task<Status> TryLogin();
        Task<Status> TryLogin(string studentId, string pwd);
        Task<(Status, string[][])> GetTable(bool reload = false);
        Task<(Status, RinkDetail)> GetRink(bool reload = false);
        Task<(Status, IEnumerable<ScoresDetail>)> GetScores(bool reload = false);
        Task<int> Evaluate();
    }
}

