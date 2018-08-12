using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Server.Shared.Core;
using Server.Shared.Models;
using WHUTSdk.Core;
using WHUTSdk.Models;

namespace Server.Service.Whut
{
    public class WhutService : IWhutService
    {
        private WhutStudent _whutStudent;
        private StudentInfo _studentInfo;
        private readonly IAccountManager<User> _manager;
        private readonly IWhutDbContext _db;

        public StudentInfo StudentInfo
        {
            get
            {
                if (_studentInfo != null) return _studentInfo;
                if (_manager.User == null) return null;
                _studentInfo = _db.FindStudent(_manager.User.Uid);
                return _studentInfo;
            }
        }

        private WhutStudent WhutStudent
        {
            get
            {
                if (_whutStudent != null) return _whutStudent;
                if (StudentInfo == null) return null;
                try
                {
                    _whutStudent = new WhutStudent(StudentInfo.StudentId, StudentInfo.Pwd);
                    return _whutStudent;
                }
                catch
                {
                    return null;
                }
            }
        }

        public WhutService(IAccountManager<User> manager, IWhutDbContext db)
        {
            _manager = manager;
            _db = db;
        }

        public WhutStatus UpdateInfo(string studentId, string pwd)
        {
            throw new NotImplementedException();
        }

        public async Task<WhutStatus> TryLogin()
        {
            if (WhutStudent == null)
                throw new Exception("Student不可为空");
            return await WhutStudent.LoginAsync();
        }

        public async Task<WhutStatus> RefreshTimeTable()
        {
            if (WhutStudent == null)
                throw new Exception("Student不可为空");
            var timeTable = await WhutStudent.GetTimeTableAsync();
            _studentInfo.TimeTable = timeTable;
            return WhutStatus.Ok;
        }

        public async Task<WhutStatus> RefreshScores()
        {
            if (WhutStudent == null)
                throw new Exception("Student不可为空");
            var (scores, rinks) = await WhutStudent.GetScoresAsync();
            _studentInfo.Scores = scores;
            _studentInfo.Rinks = rinks;
            return WhutStatus.Ok;
        }


        public Task<int> Evaluate()
        {
            throw new NotImplementedException();
        }
    }
}
