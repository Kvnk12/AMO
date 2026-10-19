using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NTTData.AMOPortal.Domain.Abstract
{
    public interface IDelRepo<T>
    {
        IQueryable<T> Where(Expression<Func<T, bool>> predicate, bool showDeleted = false);
        IQueryable<T> GetAll();
        void Restore(T o);
    }
}
