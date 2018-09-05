using LiteDB;
using Server.Shared.Core.DB;
using System;
using System.Collections.Generic;
using Server.Shared.Core;

namespace Server.DB
{
    public class BaseDbContext<T> : IBaseDbContext<T> where T : IBaseDbModel, new()
    {
        private readonly LiteCollection<T> _data;

        public BaseDbContext(LiteDatabase db, string name)
        {
            _data = db.GetCollection<T>(name);
        }

        public void Add(T data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));
            _data.Insert(data);
        }

        public T Find(string uid)
        {
            if (string.IsNullOrWhiteSpace(uid))
                throw new ArgumentException(nameof(uid));
            return _data.FindOne(x => x.Uid == uid);
        }

        public T Find(IAppUser appUser)
        {
            return Find(appUser.Uid);
        }

        public IEnumerable<T> FindAll(Func<T, bool> condition = null)
        {
            return condition == null ? _data.FindAll() : _data.Find(x => condition(x));
        }

        public bool Delete(T data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));
            return _data.Delete(data.Id);
        }

        public bool Delete(string uid)
        {
            if (string.IsNullOrWhiteSpace(uid))
                throw new ArgumentException(nameof(uid));
            return _data.Delete(x => x.Uid == uid) > 0;
        }

        public bool Delete(IAppUser appUser)
        {
            return Delete(appUser.Uid);
        }

        public int DeleteAll(Func<T, bool> condition = null)
        {

            return condition == null ? _data.Delete(x => condition(x)) : _data.Delete(x => true);
        }

        public void Update(T data)
        {
            _data.Update(data);
        }
    }
}
