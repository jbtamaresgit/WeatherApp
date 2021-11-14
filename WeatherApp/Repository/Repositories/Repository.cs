using Realms;
using Repository.Database;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Repository.Repositories
{
    public class Repository<T> : BaseRepository, IRepository<T> where T : RealmObject, new()
    {

        public Repository(IDatabaseService db) : base(db)
        {
        }

        public Realm GetRealm()
        {
            return DatabaseService.GetRealm();
        }

        public IQueryable<T> AsQueryable()
        {
            return DatabaseService.AsQueryable<T>();
        }

        public void DeleteAll()
        {
            DatabaseService.DeleteAll<T>();
        }

        public bool DeleteItem(T entity)
        {
            return DatabaseService.DeleteItem(entity);
        }


        public List<T> GetList()
        {
            return DatabaseService.GetItemList<T>();
        }

        public List<T> GetList(Func<T, bool> predicate = null)
        {
            List<T> list = new List<T>();
            list = DatabaseService.GetItemList<T>(predicate);
            return list;
        }

        public List<T> GetList<TValue>(Func<T, bool> predicate = null, Func<T, TValue> orderBy = null)
        {
            List<T> list = new List<T>();
            list = DatabaseService.GetItemList<T, TValue>(predicate, orderBy);
            return list;
        }

        public bool Insert(T item)
        {
            return DatabaseService.InsertItem(item);
        }

        public bool InsertList(IEnumerable<T> list)
        {
            DeleteAll();
            if (list == null) return false;
            return DatabaseService.InsertItems(list);
        }

        public bool Update(T item)
        {
            return DatabaseService.UpdateItem(item);
        }
    }
}
