using EFCore.BulkExtensions;
using ReestrBKS.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReestrBKS.DataAccess
{
    public abstract class Repository<T> : IRepository<T> where T : class
    {
        protected ReestrContext ReestrContext;

        public Repository(ReestrContext context)
        {
            ReestrContext = context;
        }

        public void Add(T entity)
        {
            ReestrContext.Set<T>().Add(entity);
            ReestrContext.SaveChanges();
        }

        public void AddRange(IEnumerable<T> entities)
        {
            ReestrContext.Set<T>().AddRange(entities);
            ReestrContext.SaveChanges();
        }

        public void Delete(T entity)
        {
            ReestrContext.Set<T>().Remove(entity);
            ReestrContext.SaveChanges();
        }

        public void DeleteRange(IEnumerable<T> range)
        {
            ReestrContext.Set<T>().RemoveRange(range);
            ReestrContext.SaveChanges();
        }

        public T Get(int Id)
        {
            return ReestrContext.Set<T>().Find(Id);
        }

        public IQueryable<T> GetAll()
        {
            return ReestrContext.Set<T>();
        }

        public void Update(T entity)
        {
            ReestrContext.Set<T>().Update(entity);
            ReestrContext.SaveChanges();
        }

        public void UpdateRange(IEnumerable<T> entityList)
        {
            ReestrContext.Set<T>().UpdateRange(entityList);
            ReestrContext.SaveChanges();
        }
    }
}
