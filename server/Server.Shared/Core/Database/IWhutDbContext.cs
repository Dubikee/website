using System.Collections.Generic;

namespace Server.Shared.Core.Database
{
    public interface IWhutDbContext<T> where T : IWhutStudent
    {
        IEnumerable<T> Students { get; }
        T FindStudent(string uid);
        bool AddStudent(T student);
        bool DeleteStudent(string uid);
        bool UpdateStudent(T student);
    }
}
