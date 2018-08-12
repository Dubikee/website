using System;
using System.Collections.Generic;
using System.Text;
using LiteDB;
using Server.Shared.Core;
using Server.Shared.Models;
using Server.Shared.Options;

namespace Server.DB.WhutDb
{
    public class WhutDbContext : IWhutDbContext
    {
        private readonly LiteCollection<StudentInfo> _students;

        public IEnumerable<StudentInfo> Students => _students.FindAll();

        public WhutDbContext(DbOptions opt)
        {
            _students = new LiteDatabase(opt.DbPath).GetCollection<StudentInfo>(opt.WhutCollectionName);
        }


        public StudentInfo FindStudent(string uid)
        {
            return _students.FindOne(x => x.Uid == uid);
        }

        public bool AddStudent(StudentInfo student)
        {
            return _students.Insert(student);
        }

        public bool DeleteStudent(string uid)
        {
            return _students.Delete(x => x.Uid == uid) > 0;
        }

        public bool UpdateStudent(StudentInfo student)
        {
            return _students.Update(student);
        }
    }
}
