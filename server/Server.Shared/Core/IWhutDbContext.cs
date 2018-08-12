using System;
using System.Collections.Generic;
using System.Text;
using Server.Shared.Models;

namespace Server.Shared.Core
{
    public interface IWhutDbContext
    {
        IEnumerable<StudentInfo> Students { get; }
        StudentInfo FindStudent(string uid);
        bool AddStudent(StudentInfo student);
        bool DeleteStudent(string uid);
        bool UpdateStudent(StudentInfo student);
    }
}
