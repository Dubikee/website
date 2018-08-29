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

        public bool AddStudent(WhutStudent whutStudent)
        {
            if (string.IsNullOrWhiteSpace(whutStudent.Uid))
            {
                return false;
            }
            _students.Insert(whutStudent);
            return true;
        }

        public bool DeleteStudent(string uid)
        {
            return _students.Delete(x => x.Uid == uid) > 0;
        }

        public bool UpdateStudent(WhutStudent whutStudent)
        {
            return _students.Update(whutStudent);
        }
    }
}
