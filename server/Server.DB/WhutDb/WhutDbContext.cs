using LiteDB;
using Server.Shared.Core.Database;
using Server.Shared.Models.Whut;
using Server.Shared.Options;
using System.Collections.Generic;

namespace Server.DB.WhutDb
{
    public class WhutDbContext : IWhutDbContext<WhutStudent>
    {
        private readonly LiteCollection<WhutStudent> _students;

        public IEnumerable<WhutStudent> Students => _students.FindAll();

        public WhutDbContext(DbOptions opt)
        {
            _students = new LiteDatabase(opt.DbPath).GetCollection<WhutStudent>(opt.WhutCollectionName);
        }


        public WhutStudent FindStudent(string uid)
        {
            return _students.FindOne(x => x.Uid == uid);
        }

        public bool AddStudent(WhutStudent student)
        {
            if (string.IsNullOrWhiteSpace(student.Uid))
            {
                return false;
            }
            _students.Insert(student);
            return true;
        }

        public bool DeleteStudent(string uid)
        {
            return _students.Delete(x => x.Uid == uid) > 0;
        }

        public bool UpdateStudent(WhutStudent student)
        {
            return _students.Update(student);
        }
    }
}
