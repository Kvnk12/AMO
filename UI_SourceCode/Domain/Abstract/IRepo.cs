using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NTTData.AMOPortal.Domain.Abstract
{
    public interface IRepo<T>
    {
        T Get(int id);
        IQueryable<T> GetAll();
        IQueryable<T> Where(Expression<Func<T, bool>> predicate, bool showDeleted = false);
        T Insert(T o);
        void Save();
        void Delete(T o);
        void Restore(T o);
        T Get(int id, params string[] Includes);
       // void Update(T entity);
    }
}
