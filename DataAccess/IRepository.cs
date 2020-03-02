using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace app_server.DataAccess
{
    public interface IRepository<T> : IDisposable where T : class
    {
        void Delete(Expression<Func<T, bool>> expression);
        void Delete(T item);
        void DeleteAll();
        T Single(Expression<Func<T, bool>> expression);
        System.Linq.IQueryable<T> All();
        System.Linq.IQueryable<T> All(int page, int pageSize);
        void Add(T item);
        void Add(IEnumerable<T> items);
    }
}
