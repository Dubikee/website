using System;
using System.Collections.Generic;

namespace Server.Shared.Core.DB
{
    public interface IBaseDbContext<T> where T : IBaseDbModel
    {
        void Add(T data);
        T Find(string uid);
        T Find(IAppUser appUser);
        IEnumerable<T> FindAll(Func<T, bool> condition = null);
        bool Delete(T data);
        bool Delete(string uid);
        bool Delete(IAppUser appUser);
        int DeleteAll(Func<T, bool> condition = null);
        void Update(T data);
    }
}
