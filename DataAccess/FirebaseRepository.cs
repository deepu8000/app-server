using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace app_server.DataAccess
{
    public class FirebaseRepository<T> : IRepository<T> where T : class
    {
        private FirebaseClient _provider;
        private ChildQuery _db;
        public FirebaseRepository()
        {

            _provider = new FirebaseClient("https://appdbinit.firebaseio.com");
            _db = _provider.Child(typeof(T).Name);
        }
        public void Delete(System.Linq.Expressions.Expression<Func<T, bool>> expression)
        {
            var items = All().Where(expression);
            foreach (T item in items)
            {
                Delete(item);
            }
        }
        public void Delete(T item)
        {
            // Remove the object.
            _db.<T>(x=> x.Value == item).DeleteAsync();
        }
        public void DeleteAll()
        {
            _db.DropCollection(typeof(T).Name);
        }
        public T Single(System.Linq.Expressions.Expression<Func<T, bool>> expression)
        {
            return All().Where(expression).SingleOrDefault();
        }
        public IQueryable<T> All()
        {
            return _db.GetCollection<T>(typeof(T).Name).AsQueryable();
        }
        public IQueryable<T> All(int page, int pageSize)
        {
            return _db.GetCollection<T>(typeof(T).Name).Find(FilterDefinition<T>.Empty).Skip((page - 1) * pageSize).Limit(pageSize).ToList().AsQueryable();
        }
        public void Add(T item)
        {
            _db.GetCollection<T>(typeof(T).Name).InsertOne(item);
        }
        public void Add(IEnumerable<T> items)
        {
            foreach (T item in items)
            {
                Add(item);
            }
        }
        public void Dispose() { }
    }
}
