using Realms;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Repository.Database
{
    public interface IDatabaseService
    {
        Realm GetRealm();
        IQueryable<T> AsQueryable<T>() where T : RealmObject, new();
        List<T> GetItemList<T>() where T : RealmObject, new();
        List<T> GetItemList<T>(Func<T, bool> predicate = null) where T : RealmObject, new();
        List<T> GetItemList<T, TValue>(Func<T, bool> predicate = null, Func<T, TValue> orderBy = null) where T : RealmObject, new();
        T GetItem<T>(Func<T, bool> predicate) where T : RealmObject, new();
        bool InsertItem<T>(T entity) where T : RealmObject, new();
        bool UpdateItem<T>(T entity) where T : RealmObject, new();
        bool DeleteItem<T>(T entity) where T : RealmObject, new();
        bool InsertItems<T>(IEnumerable<T> entity) where T : RealmObject, new();
        void DeleteAll<T>() where T : RealmObject, new();
    }
}
