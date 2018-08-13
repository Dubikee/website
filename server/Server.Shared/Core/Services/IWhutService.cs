using System.Threading.Tasks;
using Server.Shared.Results;

namespace Server.Shared.Core.Services
{
    public interface IWhutService<T> where T : IWhutStudent
    {
        WhutStatus UpdateInfo(string studentId, string pwd);
        T Student { get; }
        Task<WhutStatus> TryLogin();
        Task<WhutStatus> RefreshTimeTable();
        Task<WhutStatus> RefreshScores();
        Task<int> Evaluate();
    }
}
