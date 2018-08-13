using System.Threading.Tasks;
using Server.Shared.Results;

namespace Server.Shared.Core.Services
{
    public interface IWhutService<T> where T : IWhutStudent
    {
        T Student { get; }
        Task<WhutStatus> TryLogin();
        WhutStatus UpdateInfo(string studentId, string pwd);
        Task<WhutStatus> TryLogin(string studentId, string pwd);
        Task<WhutStatus> RefreshTimeTable();
        Task<WhutStatus> RefreshScores();
        Task<int> Evaluate();
    }
}
