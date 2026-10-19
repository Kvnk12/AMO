using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NTTData.AMOPortal.Domain.Abstract;
using NTTData.AMOPortal.Domain.Entities;
using Omu.ValueInjecter;
using System.Linq.Expressions;

namespace NTTData.AMOPortal.Data
{
    public class Repo<T> : IDisposable, IRepo<T> where T : Entity, new()
    {
        protected readonly DbContext dbContext;

        public Repo(IDbContextFactory f)
        {
            dbContext = f.GetContext();
        }

        public void Save()
        {
            dbContext.SaveChanges();
        }

        public T Insert(T o)
        {
            var t = dbContext.Set<T>().Create();
            t.InjectFrom(o);
            dbContext.Set<T>().Add(t);
            return t;
        }

        public virtual void Delete(T o)
        {
            //if (o is IDel)
            //    (o as IDel).IsDeleted = true;
            //else
                dbContext.Set<T>().Remove(o);
        }

        public T Get(int id)
        {
            return dbContext.Set<T>().Find(id);
        }

        public void Restore(T o)
        {
           // if (o is IDel)
             //   IoC.Resolve<IDelRepo<T>>().Restore(o);
        }

        public virtual IQueryable<T> Where(Expression<Func<T, bool>> predicate, bool showDeleted = false)
        {
           // if (typeof(IDel).IsAssignableFrom(typeof(T)))
             //   return IoC.Resolve<IDelRepo<T>>().Where(predicate, showDeleted);
            IQueryable<T> set = dbContext.Set<T>().Where(predicate);
            return set;
        }

        public virtual T FirstOrDefault(Expression<Func<T, bool>> predicate)
        {
            // if (typeof(IDel).IsAssignableFrom(typeof(T)))
            //   return IoC.Resolve<IDelRepo<T>>().Where(predicate, showDeleted);
            return dbContext.Set<T>().FirstOrDefault(predicate);
        }

        public virtual IQueryable<T> GetAll()
        {
            //if (typeof(IDel).IsAssignableFrom(typeof(T)))
              //  return IoC.Resolve<IDelRepo<T>>().GetAll();
            return dbContext.Set<T>();
        }
        //Added by Anandhi for Client Profile Begin
        public T Get(int id, params string[] Includes)
        {
            //if (typeof(IDel).IsAssignableFrom(typeof(T)))
            //  return IoC.Resolve<IDelRepo<T>>().GetAll();

            var Result = (from res in dbContext.Set<T>() where (res.Id == id) select res);

            foreach (var path in Includes)
                Result = Result.Include(path);

            List<T> Results = Result.ToList<T>();

            if (Results.Count > 0)
                return Results[0];
            else
                return null;
        }
        //Added by Anandhi for Client Profile End

        public void Dispose()
        {
            if (dbContext != null)
                dbContext.Dispose();
        }

    }
}
