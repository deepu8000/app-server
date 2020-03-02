using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace app_server.DataAccess
{
    public class MongoRepository<T> : IRepository<T> where T : class
    {
        private IMongoClient _provider;
        private IMongoDatabase _db;
        public MongoRepository()
        {

            _provider = new MongoClient("mongodb://deepu8000:Intel$12345@cluster0-shard-00-00-ukudi.mongodb.net:27017,cluster0-shard-00-01-ukudi.mongodb.net:27017,cluster0-shard-00-02-ukudi.mongodb.net:27017/test?ssl=true&replicaSet=Cluster0-shard-0&authSource=admin&retryWrites=true");
            _db = _provider.GetDatabase("poker");
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
            _db.GetCollection<T>(typeof(T).Name).DeleteOne(Builders<T>.Filter.Eq("Id", item));
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
