using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReestrBKS.DataAccess.Interfaces
{
    public interface IRepository<T>
    {
        void Add(T entity);
        void AddRange(IEnumerable<T> entities);
        void Delete(T entity);
        void DeleteRange(IEnumerable<T> range);
        T Get(int Id);
        IQueryable<T> GetAll();
        void Update(T entity);
        void UpdateRange(IEnumerable<T> entityList);
    }
}
