using Realms;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Repository.Repositories
{
    public interface IRepository<T> where T : RealmObject, new()
    {
        Realm GetRealm();
        IQueryable<T> AsQueryable();
        List<T> GetList();
        List<T> GetList(Func<T, bool> predicate = null);
        List<T> GetList<TValue>(Func<T, bool> predicate = null, Func<T, TValue> orderBy = null);
        bool Insert(T item);
        bool InsertList(IEnumerable<T> list);
        bool Update(T item);
        bool DeleteItem(T entity);
        void DeleteAll();
    }
}
