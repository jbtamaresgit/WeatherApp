using Realms;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Repository.Database
{
    public class DatabaseService : IDatabaseService
    {
        private static object _locker = new object();
        private readonly RealmConfiguration cfg;
        public DatabaseService()
        {
            lock (_locker)
            {
                cfg = new RealmConfiguration()
                {
                    SchemaVersion = 1
                };

                cfg = cfg.ConfigWithPath("WeatherApp.realm");
#if DEBUG
                cfg.ShouldDeleteIfMigrationNeeded = true;
#endif
            }
        }

        public Realm GetRealm()
        {
            return Realm.GetInstance(cfg);
        }

        public IQueryable<T> AsQueryable<T>() where T : RealmObject, new()
        {
            using (var realm = GetRealm())
            {
                return realm.All<T>();
            }
        }

        public void DeleteAll<T>() where T : RealmObject, new()
        {
            using (var realm = GetRealm())
            {
                using (var trans = realm.BeginWrite())
                {
                    realm.RemoveAll<T>();
                    trans.Commit();
                }
            }
        }

        public bool DeleteItem<T>(T entity) where T : RealmObject, new()
        {
            try
            {
                using (var realm = GetRealm())
                {
                    using (var trans = realm.BeginWrite())
                    {
                        realm.Remove(entity);
                        trans.Commit();
                    }
                }

                return true;
            }
            catch
            {
                
            }

            return false;
        }

        public T GetItem<T>(Func<T, bool> predicate) where T : RealmObject, new()
        {
            using (var realm = GetRealm())
            {
                return realm.All<T>().FirstOrDefault(predicate);
            }
        }

        public List<T> GetItemList<T>() where T : RealmObject, new()
        {
            using (var realm = GetRealm())
            {
                return realm.All<T>().ToList();
            }
        }

        public List<T> GetItemList<T>(Func<T, bool> predicate = null) where T : RealmObject, new()
        {
            using (var realm = GetRealm())
            {
                var query = realm.All<T>();

                if (predicate != null)
                    return query.Where(predicate).ToList();

                return query.ToList();
            }
        }

        public List<T> GetItemList<T, TValue>(Func<T, bool> predicate = null, Func<T, TValue> orderBy = null) where T : RealmObject, new()
        {
            using (var realm = GetRealm())
            {
                var query = realm.All<T>();

                if (predicate != null && orderBy == null)
                    return query.Where(predicate).ToList();

                if (predicate == null && orderBy != null)
                    return query.OrderBy(orderBy).ToList();

                if (predicate != null && orderBy != null)
                    return query.Where(predicate).OrderBy(orderBy).ToList();

                return query.ToList();
            }
        }

        public bool InsertItem<T>(T entity) where T : RealmObject, new()
        {
            try
            {
                using (var realm = GetRealm())
                {
                    realm.Write(() =>
                    {
                        realm.Add<T>(entity);
                    });
                }
                return true;
            }
            catch
            {

            }

            return false;
        }

        public bool InsertItems<T>(IEnumerable<T> entities) where T : RealmObject, new()
        {
            try
            {
                using (var realm = GetRealm())
                {
                    using (var trans = realm.BeginWrite())
                    {
                        foreach (var entity in entities)
                        {
                            realm.Add<T>(entity);
                        }
                        trans.Commit();
                    }
                }
                return true;
            }
            catch
            {

            }

            return false;
        }

        public bool UpdateItem<T>(T entity) where T : RealmObject, new()
        {
            try
            {
                using (var realm = GetRealm())
                {
                    using (var trans = realm.BeginWrite())
                    {
                        realm.Add<T>(entity, update: true);
                        trans.Commit();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
               
            }

            return false;
        }
    }
}
